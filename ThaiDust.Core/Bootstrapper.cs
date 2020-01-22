﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Splat;
using ThaiDust.Core.Model.Persistent;
using ThaiDust.Core.Service;

namespace ThaiDust.Core
{
    public static class Bootstrapper
    {
        public static void RegisterDatabase(string databasePath)
        {
            Locator.CurrentMutable.RegisterLazySingleton<DustContext>(() =>
            {
                var dustContext = new DustContext(databasePath);
                dustContext.Database.Migrate();
                return dustContext;
            });
        }

        public static void RegisterCoreDependencies()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Locator.CurrentMutable.RegisterLazySingleton<DustApiService>(()=>new DustApiService(new HttpClient()));
            Locator.CurrentMutable.RegisterLazySingleton<DustDataService>(()=>new DustDataService(Locator.Current.GetService<DustContext>()));
            Locator.CurrentMutable.RegisterLazySingleton<DustService>(()=>new DustService());
        }
    }
}