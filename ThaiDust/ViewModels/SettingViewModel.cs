using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ThaiDust.ViewModels
{
    public class SettingViewModel : ReactiveObject, IActivatableViewModel, IRoutableViewModel
    {
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
        public string UrlPathSegment { get; } = "setting";
        public IScreen HostScreen { get; }

        public ReactiveCommand<Unit, Unit> SaveSettingCommand { get; set; }
        public ReactiveCommand<Unit, Unit> LoadSettingCommand { get; set; }

        [Reactive] public ElementTheme CurrentTheme { get; set; } = ElementTheme.Default;

        public SettingViewModel()
        {
            LoadSettingCommand = ReactiveCommand.Create(LoadSetting);
            SaveSettingCommand = ReactiveCommand.Create(SaveSetting);

            this.WhenActivated(d =>
            {
                LoadSettingCommand.Execute().Subscribe().DisposeWith(d);
                this.WhenAnyValue(p => p.CurrentTheme).Select(_=>Unit.Default).InvokeCommand(SaveSettingCommand).DisposeWith(d);
            });
        }

        private void SaveSetting()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.RequestedTheme = CurrentTheme;

            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["Theme"] = (int)CurrentTheme;
        }

        private void LoadSetting()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            CurrentTheme = (ElementTheme)(int)localSettings.Values["Theme"];
        }
    }
}
