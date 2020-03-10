using System;
using System.Linq;
using System.Reactive.Linq;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            ViewModel = new RootPageViewModel();

            this.InitializeComponent();
            CustomTitlebar();

            Theme.IsOn = App.Current.RequestedTheme == ApplicationTheme.Dark;


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
        }

        private void CustomTitlebar()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;
            titleBar.ButtonForegroundColor = Windows.UI.Colors.Transparent;
            titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            // Set XAML element as a draggable region.
            Window.Current.SetTitleBar(AppTitleBar);
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            AppTitleBar.Height = sender.Height;
        }

        private void Theme_OnToggled(object sender, RoutedEventArgs e)
        {
            this.RequestedTheme = Theme.IsOn ? ElementTheme.Dark : ElementTheme.Light;
        }
    }
}
