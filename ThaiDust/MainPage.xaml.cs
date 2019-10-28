using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ReactiveUI;
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

        public MainPageViewModel ViewModel
        {
            get { return (MainPageViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        object IViewFor.ViewModel {
            get => ViewModel;
            set => ViewModel = (MainPageViewModel)value;
        }

        public MainPage()
        {
            ViewModel = new MainPageViewModel();

            this.InitializeComponent();

            this.WhenActivated(cleanup =>
            {
                this.OneWayBind(ViewModel, vm => vm.Stations, v => v.Stations.ItemsSource).DisposeWith(cleanup);
                this.Bind(ViewModel, vm => vm.SelectedStation, v => v.Stations.SelectedItem).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.StationParams, v => v.Parameters.ItemsSource).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.StationData, v => v.Datas.ItemsSource).DisposeWith(cleanup);
                this.Bind(ViewModel, vm => vm.SelectedParameter, v => v.Parameters.SelectedItem).DisposeWith(cleanup);

                this.Bind(ViewModel, vm => vm.StartDate, v => v.StartDate.Date).DisposeWith(cleanup);
                this.Bind(ViewModel, vm => vm.EndDate, v => v.EndDate.Date).DisposeWith(cleanup);

                this.Bind(ViewModel, vm => vm.StartTime, v => v.StartTime.Time).DisposeWith(cleanup);
                this.Bind(ViewModel, vm => vm.EndTime, v => v.EndTime.Time).DisposeWith(cleanup);

                this.BindCommand(ViewModel, vm => vm.LoadDataCommand, v => v.LoadDataButton).DisposeWith(cleanup);
            });

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
