using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SeedApp.Common.Data;
using SeedApp.Common.Exception;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using SeedApp.Common.Models;

namespace SeedApp.Data.Repositories
{
    public abstract class RepositoryBase<T>
        where T : EntityBase
    {
        protected readonly IMemberPlusApiService MmpApiService;
        protected readonly IMapper Mapper;
        protected readonly IAppDatabase Database;
        protected readonly ILogger Logger;
        protected readonly IConnectivityHelper ConnectivityHelper;
        protected readonly IAppAnalyticsProvider AppAnalyticsProvider;

        protected RepositoryBase(IAppDatabase database, IMemberPlusApiService mmpApiService, IMapper mapper, ILogger logger, IConnectivityHelper connectivityHelper, IAppAnalyticsProvider appAnalyticsProvider)
        {
            Database = database;
            MmpApiService = mmpApiService;
            Mapper = mapper;
            Logger = logger;
            ConnectivityHelper = connectivityHelper;
            AppAnalyticsProvider = appAnalyticsProvider;
        }

        protected string EndpointName { get; set; }

        public virtual void Purge()
        {
            Database.DeleteAll<T>();
        }

        protected SyncMetadata GetSyncMetadataOrDefault(string endpointName)
        {
            return Database.GetSingleOrDefaultByQuery<SyncMetadata>(s => s.EndpointName == endpointName);
        }

        protected void PurgeSyncMetadata(string endpointName)
        {
            List<SyncMetadata> syncMetadataList = Database.LoadMany<SyncMetadata>(s => s.EndpointName == endpointName);

            foreach (var syncMetadata in syncMetadataList)
                Database.Delete(syncMetadata);
        }

        protected async Task<List<T>> GetFromCacheAndRefreshFromServerAsync()
        {
            List<T> items = Database.LoadMany<T>();

            if (items.Count == 0)
            {
                Logger.Verbose($"GetFromCacheAndRefreshFromServerAsync {GetType().Name}. Refreshing data from server",
                    new { items.Count }, new[] { LoggerConstants.DbOperation });
                await RefreshDataAsync();

                items = Database.LoadMany<T>();
            }
            else
            {
                if (!ConnectivityHelper.IsConnected)
                {
                    Logger.Info("Not refreshing data in the background because we are offline",
                        new { ConnectivityState = ConnectivityHelper.IsConnected },
                        new[] { LoggerConstants.BackgroundRefresh });
                }
                else
                {
#pragma warning disable CS4014
                    Task.Run(async () =>
                    {
                        try
                        {
                            await RefreshDataAsync();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("Error occurred while refreshing data in background",
                                    new { ConnectivityState = ConnectivityHelper.IsConnected },
                                    new[] { LoggerConstants.BackgroundRefresh });
                            Logger.Exception(ex, new[] { LoggerConstants.BackgroundRefresh });

                            if (!(ex is MemberPlusApiException))
                                AppAnalyticsProvider.ReportException(ex, new[] { LoggerConstants.BackgroundRefresh });
                        }
                    });
#pragma warning restore CS4014
                }
            }

            return items;
        }

        protected void PurgeAndInsertCompleteData(IList<T> items)
        {
            if (items == null) return;

            Logger.Verbose($"PurgeAndInsertCompleteData { GetType().Name }", new { items.Count }, new[] { LoggerConstants.DbOperation });
            Database.RunInTransaction(() =>
            {
                try
                {
                    Database.DeleteAll<T>();
                    Database.InsertAll(items);
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex, new[] { LoggerConstants.DbOperation });
                }
            });
        }

        protected virtual async Task RefreshDataAsync()
        {
        }

        protected void UpdateSyncMetadata(string endpointName, DateTime? modifiedOnUtc = null, int? numberOfRecordsSynced = null, bool? isInitialRecordsSyncCompleted = null)
        {
            SyncMetadata syncMetadata = GetSyncMetadataOrDefault(endpointName) ?? new SyncMetadata
            {
                EndpointName = endpointName,
                MaxModifiedDate = DateTime.MinValue
            };

            if (modifiedOnUtc.HasValue)
                syncMetadata.MaxModifiedDate = syncMetadata.MaxModifiedDate > modifiedOnUtc ? syncMetadata.MaxModifiedDate : modifiedOnUtc;

            if (numberOfRecordsSynced.HasValue)
                syncMetadata.NumberOfRecordsSynced = numberOfRecordsSynced.Value;

            if (isInitialRecordsSyncCompleted.HasValue)
                syncMetadata.IsInitialRecordsSyncCompleted = isInitialRecordsSyncCompleted.Value;

            Database.InsertOrUpdate(syncMetadata);
        }
    }
}