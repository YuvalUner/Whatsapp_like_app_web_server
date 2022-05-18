using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DatabaseEntryModels;
using Services.DataManipulation.Interfaces;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.DataManipulation.DatabaseContextBasedImplementations {

    public class DatabaseActiveConnectionsService : IActiveConnectionsService {

        private readonly AdvancedProgrammingProjectsServerContext _context;

        public DatabaseActiveConnectionsService(AdvancedProgrammingProjectsServerContext context) {
            _context = context;
        }

        public async Task<bool> doesConnectionExist(string username, string userAgent) {
            if (await getActiveConnection(username, userAgent) != null) {
                return true;
            }
            return false;
        }

        public async Task<ActiveConnection?> getActiveConnection(string username, string userAgent) {
            
            ActiveConnection? connection = await _context.ActiveConnection.
                Where(x => x.Username == username && x.UserAgent == userAgent).FirstOrDefaultAsync();
            return connection;
        }

        public async Task<bool> removeConnection(ActiveConnection connection) {
            _context.ActiveConnection.Remove(connection);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>?> retrieveConnectionIds(string username) {

            List<string>? connectionIds = await (from connections in _context.ActiveConnection
                                         where connections.Username == username
                                         select connections.Identifier).ToListAsync();
            return connectionIds;
        }

        public async Task<bool> storeActiveConnection(ActiveConnection connection) {
            _context.ActiveConnection.Add(connection);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
