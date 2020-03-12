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
using ThaiDust.Converters;
using ThaiDust.Core.Helper;
using ThaiDust.Core.ViewModel;
using ThaiDust.Helper;
using ThaiDust.ViewModels;
using ThaiDust.Views;

namespace ThaiDust
{
    public static class Bootstrapper
    {
        public static void RegisterDependencies()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjA4MjgzQDMxMzcyZTM0MmUzMFZkTWp3R29XbTdnV2Ztbk5XemhkdDhEdmF6NkZOV2VSUU0ybDdMN1Vvbkk9");

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
            Locator.CurrentMutable.RegisterLazySingleton<IViewFor<SettingViewModel>>(()=>new SettingPage());
            //Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
            // Register BindingConvertor
            Locator.CurrentMutable.RegisterConstant(new EnumValueToRadioButtonIsCheckConverter(), typeof(IBindingTypeConverter));
            // Register Database
            Core.Bootstrapper.RegisterDatabase(dbpath);
            Core.Bootstrapper.RegisterCoreDependencies();
            // ViewModel
            Locator.CurrentMutable.RegisterLazySingleton<DashboardViewModel>(() => new DashboardViewModel());
            Locator.CurrentMutable.RegisterLazySingleton<SettingViewModel>(() => new SettingViewModel());
        }
    }
}
