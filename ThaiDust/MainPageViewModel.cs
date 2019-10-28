using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Cache;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Windows.Foundation;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Graphics.Printing.OptionDetails;
using Windows.UI.Popups;
using Windows.Web.Http;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using ThaiDust.Dto;
using ThaiDust.Models;

namespace ThaiDust
{
    public class MainPageViewModel : ReactiveObject, IActivatableViewModel
    {
        #region Private
        private readonly HttpClient _client;
        private SourceList<StationParam> _paramsList = new SourceList<StationParam>();
        private SourceList<StationValue> _values = new SourceList<StationValue>();
        #endregion

        public IList<Station> Stations = Models.Stations.All;

       [Reactive] public Station SelectedStation { get; set; }
        [Reactive] public StationParam SelectedParameter { get; set; }
        [Reactive] public DateTimeOffset? StartDate { get; set; } = DateTimeOffset.Now.AddMonths(-1);
        [Reactive] public TimeSpan? StartTime { get; set; } = TimeSpan.Zero;
        [Reactive] public DateTimeOffset? EndDate { get; set; } = DateTimeOffset.Now;
        [Reactive] public TimeSpan? EndTime { get; set; } = DateTimeOffset.Now.TimeOfDay;

        public Action<DateTime,DateTime> SetAxisAction { get; set; }

        public ObservableCollectionExtended<StationParam> StationParams = new ObservableCollectionExtended<StationParam>();
        public ObservableCollectionExtended<StationValue> StationData = new ObservableCollectionExtended<StationValue>();

        public extern int Days { [ObservableAsProperty] get; }
        public extern int Min { [ObservableAsProperty] get; }
        public extern int Max { [ObservableAsProperty] get; }
        public extern int Average { [ObservableAsProperty] get; }

        public MainPageViewModel(HttpClient httpClient = null)
        {
            _client = httpClient ?? Locator.Current.GetService<HttpClient>();

            this.WhenActivated(cleanup =>
            {
                _paramsList.Connect().Bind(StationParams).Subscribe().DisposeWith(cleanup);
                _values.Connect().Bind(StationData).Subscribe().DisposeWith(cleanup);

                var LoadParameterCommand = ReactiveCommand.CreateFromObservable<Station, IEnumerable<StationParam>>(
                    station =>
                    {
                        var dto = new GetParamListDto { StationId = station.Id };
                        return _client.TryPostAsync(new Uri("http://aqmthai.com/includes/getManReport.php"),
                                dto.GenerateFormUrlEncodedContent())
                            .ToObservable()
                            .Where(r => r.Succeeded)
                            .Select(async r => await r.ResponseMessage.Content.ReadAsStringAsync())
                            .Switch().Select(ParseParameter);
                    }).DisposeWith(cleanup);
                LoadParameterCommand.ThrownExceptions.Subscribe(ex =>
                {
                    var dialog = new MessageDialog(ex.Message, "Error");
                    dialog.ShowAsync();
                });
                LoadParameterCommand.Subscribe(r =>
                {
                    _paramsList.Clear();
                    _paramsList.AddRange(r);
                });
                this.WhenAnyValue(p => p.SelectedStation).Where(p => p != null).InvokeCommand(LoadParameterCommand);

                var canLoadDataCommand = this.WhenAnyValue(p => p.SelectedStation, p => p.SelectedParameter)
                    .Select(p => p.Item1 != null && p.Item2 != null);

                LoadDataCommand = ReactiveCommand.CreateFromObservable<IEnumerable<StationValue>>(() =>
                {
                    var startDate = new DateTime(StartDate.Value.Year,StartDate.Value.Month,StartDate.Value.Day, StartTime.Value.Hours, StartTime.Value.Minutes, StartTime.Value.Seconds);
                    var endDate = new DateTime(EndDate.Value.Year, EndDate.Value.Month, EndDate.Value.Day, EndTime.Value.Hours, EndTime.Value.Minutes, EndTime.Value.Seconds);
                    var dto = new GetDataDto { StationId = SelectedStation.Id, ParamValue = SelectedParameter.Param, StartDate = startDate, EndDate = endDate };
                    return _client.TryPostAsync(new Uri("http://aqmthai.com/includes/getMultiManReport.php"),
                            dto.GenerateFormUrlEncodedContent())
                        .ToObservable()
                        .Where(r => r.Succeeded)
                        .Select(async r => await r.ResponseMessage.Content.ReadAsStringAsync())
                        .Switch().Select(ParseData);
                }, canLoadDataCommand).DisposeWith(cleanup);

                LoadDataCommand.ThrownExceptions.Subscribe(ex =>
                {
                    var dialog = new MessageDialog(ex.Message, "Error");
                    dialog.ShowAsync();
                });

                LoadDataCommand.Subscribe(r =>
                {
                    _values.Clear();
                    IEnumerable<StationValue> stationValues = r as StationValue[] ?? r.ToArray();
                    SetAxisAction?.Invoke(stationValues.First().DateTime,stationValues.Last().DateTime);
                    _values.AddRange(stationValues);
                });
            });
        }

        public ReactiveCommand<Unit, IEnumerable<StationValue>> LoadDataCommand { get; set; }

        private IEnumerable<StationParam> ParseParameter(string xml)
        {
            var x = new XmlDocument();
            x.LoadXml(xml);
            return x.GetElementsByTagName("option").SelectMany(e =>
                e.Attributes.Where(a => a.NodeName == "value").Select(a => new StationParam
                { Param = (string)a.NodeValue, Name = (string)a.NodeValue }));
        }

        private IEnumerable<StationValue> ParseData(string xml)
        {
            var x = new XmlDocument();
            x.LoadXml(xml);
            var gregorianCalendar = new GregorianCalendar();
            return x.GetElementsByTagName("tr").SkipLast(5).Skip(1).Select(e =>
            {
                var dateNodeValue = (string)e.FirstChild.InnerText;
                int[] dateSplit = dateNodeValue.Split(',').Select(int.Parse).ToArray();
                var date = new DateTime(dateSplit[0], dateSplit[1], dateSplit[2], dateSplit[3], dateSplit[4], dateSplit[5], gregorianCalendar);
                return double.TryParse(e.LastChild?.InnerText as string, out double value) ?
                    new StationValue { DateTime = date, Value = value }
                    : new StationValue { DateTime = date, Value = null };
            });
        }
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
