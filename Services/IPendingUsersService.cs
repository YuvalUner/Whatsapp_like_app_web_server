using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Services {

    public interface IPendingUsersService {

        /// <summary>
        /// Adds a pending user to the pending users table
        /// </summary>
        /// <param name="pendingUser"></param>
        /// <returns></returns>
        Task<bool> addToPending(PendingUser pendingUser, string encryptionAlgorithm);
    }
}
