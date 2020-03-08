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
using Syncfusion.UI.Xaml.Charts;
using Telerik.Charting;
using Telerik.UI.Xaml.Controls.Chart;
using ThaiDust.Core.Dto;
using ThaiDust.Core.Model.Persistent;
using ThaiDust.Core.ViewModel;
using ThaiDust.ViewModels;
using Grid = ThaiDust.Helper.Grid;

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

        private string Today { get; } = DateTime.Today.ToString();
        private string Tomorrow { get; } = DateTime.Today.AddDays(1).ToString();

        public DashboardPage()
        {

            this.InitializeComponent();

            PM25Chart.Annotations.Add(new HorizontalLineAnnotation
                {Y1 = 0.5, CoordinateUnit = CoordinateUnit.Axis, ShowAxisLabel = true});

            this.WhenActivated(cleanup =>
            {
                this.OneWayBind(ViewModel, vm => vm.ManagedStations, v => v.Stations.ItemsSource).DisposeWith(cleanup);
                this.Bind(ViewModel, vm => vm.SelectedStation, v => v.Stations.SelectedItem).DisposeWith(cleanup);
                //ViewModel.StationData.ObserveCollectionChanges().Select(_ =>
                //    var result =
                //        from d in ViewModel.StationData
                //        group d by d.DateTime.Date
                //        into g
                //        orderby g.Key
                //        select g;
                //    var cws = new CollectionViewSource { Source = result, IsSourceGrouped = true };
                //    return cws.View;
                this.OneWayBind(ViewModel, vm => vm.SelectedStation.Code, v => v.StationCode.Text).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.SelectedStation.Name, v => v.StationName.Text).DisposeWith(cleanup);

                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.PM25.Text, Selector(RecordType.PM25)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.PM10.Text, Selector(RecordType.PM10)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.SO2.Text, Selector(RecordType.SO2)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.O3.Text, Selector(RecordType.O3)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.NO2.Text, Selector(RecordType.NO2)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.CO.Text, Selector(RecordType.CO)).DisposeWith(cleanup);

                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.PM25Date.Text, DateSelector(RecordType.PM25)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.PM10Date.Text, DateSelector(RecordType.PM10)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.SO2Date.Text, DateSelector(RecordType.SO2)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.O3Date.Text, DateSelector(RecordType.O3)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.NO2Date.Text, DateSelector(RecordType.NO2)).DisposeWith(cleanup);
                this.OneWayBind(ViewModel, vm => vm.LastRecords, v => v.CODate.Text, DateSelector(RecordType.CO)).DisposeWith(cleanup);

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
        private Func<IEnumerable<Record>, string> DateSelector(RecordType type)
        {
            return (IEnumerable<Record> arg) =>
            {
                if (arg == null || !arg.Any()) return "--";
                var val = arg.FirstOrDefault(r => r?.Type == type);
                return (val != null) ? (val.DateTime.Date == DateTime.Today ? val.DateTime.ToString("HH:mm") : val.DateTime.ToString("yyyy MMMM dd HH:mm")) : "--" ;
            };
        }

        public IEnumerable<Record> GetData(ObservableCollectionExtended<Record> source, RecordType recordType)
        {
            return source.Where(r => r.Type == recordType);
        }

        public IEnumerable<Record> GetTodayData(ObservableCollectionExtended<Record> source, RecordType recordType)
        {
            var b = source.Where(r => r.Type == recordType && r.DateTime.Date == DateTime.Today);
            return b;
        }

        private void Hamburger_OnClick(object sender, RoutedEventArgs e)
        {
            DashboardSplitView.IsPaneOpen = !DashboardSplitView.IsPaneOpen;
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
