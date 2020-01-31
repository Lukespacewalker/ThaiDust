﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ReactiveUI;
using Splat;
using ThaiDust.Core.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ThaiDust
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

            this.WhenActivated(cleanup =>
            {
                this.OneWayBind(ViewModel, vm => vm.Router, v => v.RoutedViewHost.Router)
                    .DisposeWith(cleanup);
                var navViewItemInvokedObservable = Observable
                    .FromEvent<TypedEventHandler<NavigationView, NavigationViewItemInvokedEventArgs>,
                        (NavigationView,NavigationViewItemInvokedEventArgs)>(
                        rxHandler => (navigationView, navigationViewItemInvokedEventArgs) => rxHandler((navigationView, navigationViewItemInvokedEventArgs)),
                        handler => RootNavigationView.ItemInvoked += handler,
                        handler => RootNavigationView.ItemInvoked -= handler);
                navViewItemInvokedObservable.Select(e =>
                {
                    var (_, navigationViewItemInvokedEventArgs) = e;
                    return navigationViewItemInvokedEventArgs.IsSettingsInvoked ? "Setting" : (navigationViewItemInvokedEventArgs.InvokedItem as string);
                }).InvokeCommand(ViewModel.NavigateCommand).DisposeWith(cleanup);
            });
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

        private void NavigationView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}