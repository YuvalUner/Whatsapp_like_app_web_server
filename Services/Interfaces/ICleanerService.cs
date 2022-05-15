using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services {
    /// <summary>
    /// A service for cleaning the database of records that have passed the "expiry date" we've decided for them.
    /// Currently unusable because I can not use IHostedService or BackgroundTask, due to NuGet packages
    /// limitations imposed in the instructions.
    /// TODO: Add them when no longer limited.
    /// </summary>
    public interface ICleanerService {

        /// <summary>
        /// Begins all the tasks the service provides, instead of 1 by one by the user.
        /// </summary>
        /// <param name="sleepTimeUsers"></param>
        /// <param name="sleepTimeCodes"></param>
        public Task beginTasks(int sleepTimeUsers = 10 * 60 * 1000, int sleepTimeCodes = 60 * 1000);

        /// <summary>
        /// Cleans the database of old verification codes (over 30 minutes old).
        /// </summary>
        /// <param name="sleepTime">The amount of time to sleep between each cleaning of the database.
        /// Defaults to 1 minute.</param>
        public void CleanOldVerificationCodes(int sleepTime = 60 * 1000);

        /// <summary>
        /// Cleans pending users that are over a day old from the database and enables users to 
        /// sign up with those usernames again.
        /// </summary>
        /// <param name="sleepTime">The amount of time between each cleaning of the database.
        /// Defaults to 10 minutes.</param>
        public void CleanOldPendingUsers(int sleepTime = 10 * 60 * 1000);

    }
}
