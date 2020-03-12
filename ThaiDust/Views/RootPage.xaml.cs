using System;
using System.Linq;
using System.Reactive.Linq;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using ReactiveUI;
using ThaiDust.Core.ViewModel;
using ThaiDust.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ThaiDust.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RootPage : Page, IViewFor<RootPageViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(RootPageViewModel), typeof(RootPage), new PropertyMetadata(default(RootPageViewModel)));

        public RootPageViewModel ViewModel {
            get { return (RootPageViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        object IViewFor.ViewModel {
            get => ViewModel;
            set => ViewModel = (RootPageViewModel)value;
        }


        public RootPage()
        {
            // Load Theme from setting
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.Keys.Any(s => s == "Theme"))
            {
                (Window.Current.Content as Frame).RequestedTheme =
                    (ElementTheme)(int)Windows.Storage.ApplicationData.Current.LocalSettings.Values["Theme"];
            }
            ViewModel = new RootPageViewModel();

            this.InitializeComponent();
            CustomTitlebar();

            this.OneWayBind(ViewModel, vm => vm.Router, v => v.RoutedViewHost.Router);
            ViewModel.Router.CurrentViewModel.Where(vm => vm is IViewModelInfo).Select(vm => (vm as IViewModelInfo).Title)
                .BindTo(this, v => v.RootNavigationView.Header);

            var navViewItemInvokedObservable = Observable
                .FromEvent<TypedEventHandler<NavigationView, NavigationViewItemInvokedEventArgs>,
                    (NavigationView, NavigationViewItemInvokedEventArgs)>(
                    rxHandler => (navigationView, navigationViewItemInvokedEventArgs) => rxHandler((navigationView, navigationViewItemInvokedEventArgs)),
                    handler => RootNavigationView.ItemInvoked += handler,
                    handler => RootNavigationView.ItemInvoked -= handler);
            navViewItemInvokedObservable.Select(e =>
            {
                var (_, navigationViewItemInvokedEventArgs) = e;
                return navigationViewItemInvokedEventArgs.IsSettingsInvoked
                    ? "Setting"
                    : (navigationViewItemInvokedEventArgs.InvokedItem as string);
            }).InvokeCommand(ViewModel.NavigateCommand);

            RootNavigationView.SelectedItem = RootNavigationView.MenuItems.First();
            ViewModel.NavigateCommand.Execute("Dashboard").Take(1).Subscribe();
            this.ActualThemeChanged += RootPage_ActualThemeChanged;
        }

        private void RootPage_ActualThemeChanged(FrameworkElement sender, object args)
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            if (sender.ActualTheme == ElementTheme.Dark)
            {
                titleBar.ButtonForegroundColor = Colors.White;
            }
            else
            {
                titleBar.ButtonForegroundColor = Colors.Black;
            }
        }

        private void CustomTitlebar()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;
            titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            if (Application.Current.Resources.TryGetValue("TitlebarButtonForeground", out object value))
            {
                titleBar.ButtonForegroundColor = (Color)value;
            }
            coreTitleBar.ExtendViewIntoTitleBar = true;
            //coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            // Set XAML element as a draggable region.
            Window.Current.SetTitleBar(AppTitleBar);
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            //AppTitleBar.Height = sender.Height;
        }
    }
}
