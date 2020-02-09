using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using ThaiDust.Core.Helper;
using ThaiDust.Core.Model;
using ThaiDust.Core.Model.Persistent;
using ThaiDust.Core.Service;

namespace ThaiDust.Core.ViewModel
{
    public class DashboardViewModel : ReactiveObject, IRoutableViewModel, IActivatableViewModel, IViewModelInfo
    {
        #region Private
        private readonly ExcelGenerator _excelGenerator;
        private readonly DustService _dustService;
        private SourceList<StationParam> _paramsList = new SourceList<StationParam>();
        private SourceList<Record> _values = new SourceList<Record>();
        #endregion

        public string Title { get; } = "Dustboard";

        public IList<Station> ManagedStations => _dustService.ManagedStations;

        [Reactive] public Station SelectedStation { get; set; }
        [Reactive] public DashboardInfo Info { get; set; }

        [Reactive] public StationParam SelectedParameter { get; set; }
        [Reactive] public DateTimeOffset? StartDate { get; set; } = DateTimeOffset.Now.AddMonths(-1);
        [Reactive] public TimeSpan? StartTime { get; set; } = TimeSpan.Zero;
        [Reactive] public DateTimeOffset? EndDate { get; set; } = DateTimeOffset.Now;
        [Reactive] public TimeSpan? EndTime { get; set; } = DateTimeOffset.Now.TimeOfDay;

        public Action<DateTime, DateTime> SetAxisAction { get; set; }
        public ObservableCollectionExtended<StationParam> StationParams = new ObservableCollectionExtended<StationParam>();
        public ObservableCollectionExtended<Record> StationData = new ObservableCollectionExtended<Record>();

        [Reactive] public int Days { get; private set; }
        [Reactive] public double Min { get; private set; }
        [Reactive] public double Max { get; private set; }
        [Reactive] public double Average { get; private set; }

        public DashboardViewModel(DustService dustService = null, ExcelGenerator excelGenerator = null, IScreen screen = null)
        {
            _excelGenerator = excelGenerator ?? Locator.Current.GetService<ExcelGenerator>();
            _dustService = dustService ?? Locator.Current.GetService<DustService>();
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            this.WhenActivated(cleanup =>
            {
                _values.Connect().Bind(StationData).Subscribe().DisposeWith(cleanup);

                var canLoadDataCommand = this.WhenAnyValue(p => p.SelectedStation)
                    .Select(p => p != null);

                LoadDataCommand = ReactiveCommand.CreateFromObservable<Station, Record[]>(station =>
                {
                    Info = null;
                    _values.Clear();

                    IObservable<Record[]> s = _dustService.GetAvailableParametersAsync(station)
                        .Select(@params => @params.Select(p => p.Param).ToArray())
                        .Select(@params => _dustService.GetStationRecordsAsync(station.Code, @params)).Switch();

                    // var startDate = new DateTime(StartDate.Value.Year, StartDate.Value.Month, StartDate.Value.Day, StartTime.Value.Hours, StartTime.Value.Minutes, StartTime.Value.Seconds);
                    //var endDate = new DateTime(EndDate.Value.Year, EndDate.Value.Month, EndDate.Value.Day, EndTime.Value.Hours, EndTime.Value.Minutes, EndTime.Value.Seconds);
                    return s;
                }, canLoadDataCommand).DisposeWith(cleanup);

                LoadDataCommand.ThrownExceptions.Subscribe(ShowError);

                LoadDataCommand.Subscribe(records =>
                {
                    if (records.Length > 0)
                    {
                        var lastRecord = records.Last();
                        Info ??= new DashboardInfo { CurrentDateTime = lastRecord.DateTime };
                        Info.PM25 = lastRecord.Type switch
                        {
                            RecordType.PM25 => lastRecord.Value,
                            RecordType.PM10 => lastRecord.Value,
                            RecordType.NO2 => lastRecord.Value,
                            RecordType.CO => lastRecord.Value,
                            RecordType.O3 => lastRecord.Value,
                            RecordType.SO2 => lastRecord.Value,
                            _ => Info.PM25
                        };
                        SetAxisAction?.Invoke(records.First().DateTime, records.Last().DateTime);
                    }
                    // Summarize
                    //Days = records.GroupBy(p => p.DateTime.Date).Count();
                    //Min = records.Where(p => p.Value != null).Min(p => p.Value).Value;
                    //Max = records.Where(p => p.Value != null).Max(p => p.Value).Value;
                    //Average = Math.Round(records.Where(p => p.Value != null).Average(p => p.Value).Value, 2);

                    _values.AddRange(records);
                });

                var canSaveToExcelCommand = StationData.ObserveCollectionChanges().Select(_ => StationData.Count > 0);

                SaveToExcelCommand = ReactiveCommand.CreateFromTask<IEnumerable<Record>>(data => _excelGenerator.CreateExcel(SelectedStation.Code, data), canSaveToExcelCommand);

                SaveToExcelCommand.ThrownExceptions.Subscribe(ShowError);


                this.WhenAnyValue(p => p.SelectedStation).Where(p => p != null).InvokeCommand(LoadDataCommand);
            });
        }

        private void ShowError(Exception ex)
        {
            //var dialog = new MessageDialog(ex.Message, "Error");
            //dialog.ShowAsync();
        }

        public ReactiveCommand<IEnumerable<Record>, Unit> SaveToExcelCommand { get; set; }

        public ReactiveCommand<Station, Record[]> LoadDataCommand { get; set; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
        public string UrlPathSegment { get; } = "dashboard";
        public IScreen HostScreen { get; }
    }
}
