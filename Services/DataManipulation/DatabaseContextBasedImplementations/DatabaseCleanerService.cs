using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Data;
using Domain.DatabaseEntryModels;
using Microsoft.EntityFrameworkCore;
using Services.DataManipulation.Interfaces;

namespace Services.DataManipulation.DatabaseContextBasedImplementations {
    public class DatabaseCleanerService : ICleanerService {

        private readonly AdvancedProgrammingProjectsServerContext _context;

        public DatabaseCleanerService(AdvancedProgrammingProjectsServerContext context) {
            this._context = context;
        }

        public Task beginTasks(int sleepTimeUsers = 10 * 60 * 1000, int sleepTimeCodes = 60 * 1000) {
            Thread usersCleaner = new Thread(() => CleanOldPendingUsers(sleepTimeUsers));
            Thread codesCleaner = new Thread(() => CleanOldVerificationCodes(sleepTimeCodes));
            usersCleaner.Start();
            codesCleaner.Start();
            return Task.CompletedTask;
        }

        public void CleanOldPendingUsers(int sleepTime = 10 * 60 * 1000) {
            while (true) {
                lock (_context) {
                    List<PendingUser> pendingUsers = _context.PendingUser.ToList();
                    foreach (PendingUser user in pendingUsers) {
                        if (DateTime.UtcNow.Subtract(user.timeCreated).TotalDays >= 1) {
                            pendingUsers.Remove(user);
                            _context.Entry(user).State = EntityState.Deleted;
                        }
                    }
                    _context.SaveChanges();
                }
                Thread.Sleep(sleepTime);

            }
        }

        public async void CleanOldVerificationCodes(int sleepTime = 60 * 1000) {
            while (true) {

                lock (_context) {
                    List<PendingUser> pendingUsers = _context.PendingUser.ToList();
                    foreach (PendingUser user in pendingUsers) {
                        if (user.verificationCode != null) {
                            if (DateTime.UtcNow.Subtract(user.verificationCodeCreationTime).TotalMinutes >= 30) {
                                user.verificationCode = null;
                                user.verificationCodeCreationTime = DateTime.UtcNow.AddDays(30);
                                _context.Entry(user).State = EntityState.Modified;
                            }
                        }
                    }
                    _context.SaveChanges();
                }
                Thread.Sleep(sleepTime);
            }
        }
    }
}
