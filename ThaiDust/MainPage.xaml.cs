using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
using DynamicData.Binding;
using ReactiveUI;
using Telerik.Charting;
using Telerik.UI.Xaml.Controls.Chart;
using MainPage = ThaiDust.MainPage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ThaiDust
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IViewFor<MainPageViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(MainPageViewModel), typeof(MainPage), new PropertyMetadata(default(MainPageViewModel)));

        public MainPageViewModel ViewModel {
            get { return (MainPageViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        object IViewFor.ViewModel {
            get => ViewModel;
            set => ViewModel = (MainPageViewModel)value;
        }

        private MainPageViewModel ViewModel2 => ViewModel;

        public MainPage()
        {
            ViewModel = new MainPageViewModel();
            ViewModel.SetAxisAction = SetAxis;
            this.InitializeComponent();
            CustomTitlebar();

            this.WhenActivated(cleanup =>
            {
                this.OneWayBind(ViewModel, vm => vm.Stations, v => v.Stations.ItemsSource).DisposeWith(cleanup);
                this.Bind(ViewModel, vm => vm.SelectedStation, v => v.Stations.SelectedItem).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.StationParams, v => v.Parameters.ItemsSource).DisposeWith(cleanup);
                ViewModel.StationData.ObserveCollectionChanges().Select(_ =>
                {
                    var result =
                        from d in ViewModel.StationData
                        group d by d.DateTime.Date
                        into g
                        orderby g.Key
                        select g;
                    var cws = new CollectionViewSource { Source = result, IsSourceGrouped = true };
                    return cws.View;
                }).BindTo(this, v => v.StationData.ItemsSource).DisposeWith(cleanup);

                this.Bind(ViewModel, vm => vm.SelectedParameter, v => v.Parameters.SelectedItem).DisposeWith(cleanup);

                this.Bind(ViewModel, vm => vm.StartDate, v => v.StartDate.Date).DisposeWith(cleanup);
                this.Bind(ViewModel, vm => vm.EndDate, v => v.EndDate.Date).DisposeWith(cleanup);

                this.Bind(ViewModel, vm => vm.StartTime, v => v.StartTime.Time).DisposeWith(cleanup);
                this.Bind(ViewModel, vm => vm.EndTime, v => v.EndTime.Time).DisposeWith(cleanup);

                this.BindCommand(ViewModel, vm => vm.LoadDataCommand, v => v.LoadDataButton).DisposeWith(cleanup);

                this.OneWayBind(ViewModel, vm => vm.StationData, v => v.Chart.DataContext).DisposeWith(cleanup);
            });
        }


        private void SetAxis(DateTime minimum, DateTime maximum)
        {
            Axis.Minimum = minimum;
            Axis.Maximum = maximum;
            Axis.LabelFormatter = new DateLabelFormatter();
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


    }

    public class DateLabelFormatter : IContentFormatter
    {
        public object Format(object owner, object content)
        {
            // The owner parameter is the Axis instance which labels are currently formatted
            var axis = owner as Axis;
            return ((DateTime)content).ToString("dd/MM/yyyy");
        }
    }


    public static class TimePickerHelper
    {
        public static IObservable<EventPattern<TimePickerValueChangedEventArgs>> GetTimePickerObservable(this TimePicker picker)
        {
            return Observable.FromEventPattern<TimePickerValueChangedEventArgs>(
                handler => picker.TimeChanged += handler, handler => picker.TimeChanged -= handler);
        }
    }
}
