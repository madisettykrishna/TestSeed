using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using SeedApp.Common.Utilities;
using SeedApp.Data.Interfaces;

namespace SeedApp.Data.Managers
{
    public class PeriodicSyncManager : IPeriodicSyncManager
    {
        private readonly IList<IRequirePeriodicSync> _syncProviders;
        private readonly ILogger _logger;
        private readonly IConnectivityHelper _connectivityHelper;
        private readonly AsyncLock _lock = new AsyncLock();
        private bool _isInitialized;
        private bool _explicitPausingRequest;

        public PeriodicSyncManager(ILogger logger,
            IConnectivityHelper connectivityHelper)
        {
            _logger = logger;
            _connectivityHelper = connectivityHelper;
            _syncProviders = new List<IRequirePeriodicSync>
            {
            };
        }

        public void ContinueSyncing()
        {
            lock (_lock)
            {
                _logger.Info("Periodic syncing unpaused", null, new[] { LoggerConstants.PeriodicSyncing });
                _explicitPausingRequest = false;
            }
        }

        public void InitiateSyncing()
        {
            _logger.Info("Trying to initiate periodic syncing", null, new[] { LoggerConstants.PeriodicSyncing });
            lock (_lock)
            {
                if (_isInitialized)
                {
                    _logger.Info("Periodic syncing is already initiated", null, new[] { LoggerConstants.PeriodicSyncing });
                    return;
                }

                _isInitialized = true;
            }

            _explicitPausingRequest = false;

            // KB: The _initiated flag with locking ensure the following for loop is executed only once
            foreach (var syncProvider in _syncProviders)
            {
                try
                {
                    StartBackgroundTasksForEachSyncProviders(syncProvider);
                }
                catch (Exception ex)
                {
                    _logger.Exception(ex, new[] { LoggerConstants.PeriodicSyncing });
                }
            }

            _logger.Info("Periodic syncing started. This message should appear once per app session.", null, new[] { LoggerConstants.PeriodicSyncing });
        }

        public void PauseSyncing()
        {
            lock (_lock)
            {
                _logger.Info("Periodic syncing paused", null, new[] { LoggerConstants.PeriodicSyncing });
                _explicitPausingRequest = true;
            }
        }

        private void StartBackgroundTasksForEachSyncProviders(IRequirePeriodicSync syncProvider)
        {
            _logger.Info("Creating background tasks for " + syncProvider.FriendlyNameForLogs + ". This message should appear once per sync provider per app session.", new { name = syncProvider.FriendlyNameForLogs }, new[] { LoggerConstants.PeriodicSyncing });

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    if (!_explicitPausingRequest && _connectivityHelper.IsConnected)
                    {
                        try
                        {
                            _logger.Info(syncProvider.FriendlyNameForLogs + ".SynchronizeAsync method executing", new { ExplicitPausingRequested = _explicitPausingRequest, ConnectivityState = _connectivityHelper.IsConnected }, new[] { LoggerConstants.PeriodicSyncing });
                            await syncProvider.SynchronizeAsync();
                        }
                        catch (Exception ex)
                        {
                            //// KB: SynchronizeAsync may raise exception when n/w connectivity is lost during n/w calls. 
                            //// We want to record and then swallow it bcoz we dont want the periodic sync manager to fail
                            _logger.Exception(ex, new[] { LoggerConstants.PeriodicSyncing });
                        }
                    }
                    else
                    {
                        _logger.Info(syncProvider.FriendlyNameForLogs + ".SynchronizeAsync method will be skipped", new { ExplicitPausingRequested = _explicitPausingRequest, ConnectivityState = _connectivityHelper.IsConnected }, new[] { LoggerConstants.PeriodicSyncing });
                    }

                    _logger.Info(syncProvider.FriendlyNameForLogs + " task going to sleep", new { Interval = syncProvider.IntervalInSecs }, new[] { LoggerConstants.PeriodicSyncing });
                    await Task.Delay(1000 * syncProvider.IntervalInSecs);
                }
            });
        }
    }
}