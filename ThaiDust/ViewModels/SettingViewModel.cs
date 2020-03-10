using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ThaiDust.ViewModels
{
    public class SettingViewModel : ReactiveObject, IActivatableViewModel, IRoutableViewModel
    {
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
        public string UrlPathSegment { get; } = "setting";
        public IScreen HostScreen { get; }

        [Reactive] public ElementTheme CurrentTheme { get; set; } = ElementTheme.Default;

        private void SaveSetting()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["Theme"] = CurrentTheme;
        }

        private void LoadSetting()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            CurrentTheme =  (ElementTheme)(int)localSettings.Values["Theme"];
        }
    }
}
