using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;
using Splat;
using ThaiDust.Helper;

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
                catch (Exception e)
                {
                }
            }
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "dust.db");
            Core.Bootstrapper.RegisterDatabase(dbpath);
            Core.Bootstrapper.RegisterCoreDependencies();
            Locator.CurrentMutable.RegisterLazySingleton(()=>new ExcelGenerator());
        }
    }
}
