using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Splat;

namespace ThaiDust
{
    public static class Ioc
    {
        public static void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterLazySingleton(()=>new HttpClient());
        }
    }
}
