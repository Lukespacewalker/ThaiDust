using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Splat;
using ThaiDust.Core.Model.Persistent;
using ThaiDust.Core.Service;
using ThaiDust.Core.ViewModel;

namespace ThaiDust.Core
{
    public static class Bootstrapper
    {
        public static void RegisterDatabase(string databasePath)
        {
            Locator.CurrentMutable.RegisterLazySingleton<DustContext>(() =>
            {
                var dustContext = new DustContext(databasePath);
                //dustContext.Database.EnsureDeleted();
                dustContext.Database.Migrate();
                dustContext.ChangeTracker.AutoDetectChangesEnabled = false;
                return dustContext;
            });
        }

        public static void RegisterCoreDependencies()
        {
            // Enable Support of TIS-620 codepage
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // Application Instance
            Locator.CurrentMutable.RegisterConstant(new Instance());
            // Service
            Locator.CurrentMutable.RegisterLazySingleton<ThaiPollutionControlDataApi>(()=>new ThaiPollutionControlDataApi(new HttpClient()));
            Locator.CurrentMutable.RegisterLazySingleton<DustDataService>(()=>new DustDataService(Locator.Current.GetService<DustContext>()));
            Locator.CurrentMutable.RegisterLazySingleton<DustService>(()=>new DustService());
            // RegisterViewModel
            Locator.CurrentMutable.RegisterLazySingleton<StationManagerViewModel>(() => new StationManagerViewModel());
        }
    }
}
