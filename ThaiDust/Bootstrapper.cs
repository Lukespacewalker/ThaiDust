using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;
using ReactiveUI;
using Splat;
using ThaiDust.Core.Helper;
using ThaiDust.Core.ViewModel;
using ThaiDust.Helper;
using ThaiDust.ViewModels;

namespace ThaiDust
{
    public static class Bootstrapper
    {
        public static void RegisterDependencies()
        {
            var task = ApplicationData.Current.LocalFolder.CreateFileAsync("dust.db", CreationCollisionOption.OpenIfExists).AsTask();
            if (!task.IsCompleted)
            {
                try
                {
                    task.RunSynchronously();
                }
                catch (InvalidOperationException) { }
            }
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "dust.db");
            // Register Platform-dependent services
            Locator.CurrentMutable.RegisterConstant<IFilePicker>(new FilePicker());
            Locator.CurrentMutable.RegisterLazySingleton(() => new ExcelGenerator());
            // Register all view using Assembly scanning
            Locator.CurrentMutable.RegisterLazySingleton<IViewFor<DashboardViewModel>>(()=>new DashboardPage());
            Locator.CurrentMutable.RegisterLazySingleton<IViewFor<StationManagerViewModel>>(()=>new StationManagerPage());
            //Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
            // Register Database
            Core.Bootstrapper.RegisterDatabase(dbpath);
            Core.Bootstrapper.RegisterCoreDependencies();
        }
    }
}
