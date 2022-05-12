using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Services {

    public class DatabaseRegisteredUsersService : IRegisteredUsersService {

        private readonly AdvancedProgrammingProjectsServerContext _context;

        public DatabaseRegisteredUsersService(AdvancedProgrammingProjectsServerContext context) {

            this._context = context;

        }

        public async Task<bool> doesUserExists(string? username) {
            if (username == null) {
                return false;
            }
            if (await this.GetRegisteredUser(username) == null) {
                return false;
            }
            return true;
        }

        public async Task<RegisteredUser?> GetRegisteredUser(string? username) {
            if (username == null) {
                return null;
            }
            RegisteredUser? user = await _context.RegisteredUser.Where(ru => ru.username == username).FirstOrDefaultAsync();
            return user;
        }
    }
}
