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
using DocumentFormat.OpenXml.Wordprocessing;
using DynamicData.Binding;
using ReactiveUI;
using Telerik.Charting;
using Telerik.UI.Xaml.Controls.Chart;
using ThaiDust.Core.Dto;
using ThaiDust.Core.Model.Persistent;
using ThaiDust.Core.ViewModel;
using ThaiDust.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ThaiDust
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardPage : Page, IViewFor<DashboardViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(DashboardViewModel), typeof(DashboardPage), new PropertyMetadata(default(DashboardViewModel)));

        public DashboardViewModel ViewModel {
            get { return (DashboardViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        object IViewFor.ViewModel {
            get => ViewModel;
            set => ViewModel = (DashboardViewModel)value;
        }

        public DashboardPage()
        {

            this.InitializeComponent();


            this.WhenActivated(cleanup =>
            {
                this.OneWayBind(ViewModel, vm => vm.ManagedStations, v => v.Stations.ItemsSource).DisposeWith(cleanup);
                this.Bind(ViewModel, vm => vm.SelectedStation, v => v.Stations.SelectedItem).DisposeWith(cleanup);
                //this.OneWayBind(ViewModel, vm => vm.StationParams, v => v.Parameters.ItemsSource).DisposeWith(cleanup);

                //ViewModel.StationData.ObserveCollectionChanges().Select(_ =>
                //{
                //    var result =
                //        from d in ViewModel.StationData
                //        group d by d.DateTime.Date
                //        into g
                //        orderby g.Key
                //        select g;
                //    var cws = new CollectionViewSource { Source = result, IsSourceGrouped = true };
                //    return cws.View;
                //}).BindTo(this, v => v.StationData.ItemsSource).DisposeWith(cleanup);

                //this.Bind(ViewModel, vm => vm.StartDate, v => v.StartDate.Date).DisposeWith(cleanup);
                //this.Bind(ViewModel, vm => vm.EndDate, v => v.EndDate.Date).DisposeWith(cleanup);

                //this.Bind(ViewModel, vm => vm.StartTime, v => v.StartTime.Time).DisposeWith(cleanup);
                //this.Bind(ViewModel, vm => vm.EndTime, v => v.EndTime.Time).DisposeWith(cleanup);

                //this.BindCommand(ViewModel, vm => vm.LoadDataCommand, v => v.LoadDataButton).DisposeWith(cleanup);

                this.OneWayBind(ViewModel, vm => vm.SelectedStation.Code, v => v.StaionCode.Text).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.SelectedStation.Name, v => v.StaionCode.Text).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.PM25.Text, Selector(RecordType.PM25)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.PM10.Text, Selector(RecordType.PM10)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.SO2.Text, Selector(RecordType.SO2)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.O3.Text, Selector(RecordType.O3)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.NO2.Text, Selector(RecordType.NO2)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.CO.Text, Selector(RecordType.CO)).DisposeWith(cleanup);

                this.BindCommand(ViewModel, vm => vm.SaveToExcelCommand, v => v.ExportButton, vm => vm.StationData).DisposeWith(cleanup);
            }); 
        }

        private Func< IEnumerable<Record>,string> Selector(RecordType type)
        {
            return (IEnumerable<Record> arg) =>
            {
                if (arg == null || !arg.Any()) return "--";
                var val = arg.FirstOrDefault(r => r?.Type == type);
                return (val != null) ? (val.Value.HasValue ? val.Value.ToString() : "--") : "--";
            };
        }

        public IEnumerable<Record> GetData(ObservableCollectionExtended<Record> source, RecordType recordType)
        {
            return source.Where(r => r.Type == recordType);
        }

        private void SetAxis(DateTime minimum, DateTime maximum)
        {

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
