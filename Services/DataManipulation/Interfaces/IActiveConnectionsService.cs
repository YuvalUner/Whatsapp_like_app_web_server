using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DatabaseEntryModels;

namespace Services.DataManipulation.Interfaces {


    public interface IActiveConnectionsService {

        /// <summary>
        /// Stores an active connection in the database.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        Task<bool> storeActiveConnection(ActiveConnection connection);

        /// <summary>
        /// Returns whether a connection already exists or not.
        /// </summary>
        /// <returns></returns>
        Task<bool> doesConnectionExist(string username, string userAgent);

        Task<bool> removeConnection(ActiveConnection connection);

        Task<ActiveConnection> getActiveConnection(string username, string userAgent);

        Task<List<string>?> retrieveConnectionIds(string username);
    }
}
