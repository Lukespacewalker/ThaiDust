using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public abstract class DashboardViewModelCore : ReactiveObject, IActivatableViewModel, IViewModelInfo
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
        //[Reactive] public DashboardInfo Info { get; set; }
        //private ReadOnlyObservableCollection<Record> _lastestRecords;
        [Reactive] public IEnumerable<Record> LastRecords { get; set; }

        //[Reactive] public ObservableCollectionExtended<Record> LastestRecords { get; set; } = new ObservableCollectionExtended<Record>(); 

        [Reactive] public DateTimeOffset? StartDate { get; set; } = DateTimeOffset.Now.AddMonths(-1);
        [Reactive] public TimeSpan? StartTime { get; set; } = TimeSpan.Zero;
        [Reactive] public DateTimeOffset? EndDate { get; set; } = DateTimeOffset.Now;
        [Reactive] public TimeSpan? EndTime { get; set; } = DateTimeOffset.Now.TimeOfDay;

        public ObservableCollectionExtended<Record> StationData = new ObservableCollectionExtended<Record>();

        public DashboardViewModelCore(DustService dustService = null, ExcelGenerator excelGenerator = null, IScreen screen = null)
        {
            _excelGenerator = excelGenerator ?? Locator.Current.GetService<ExcelGenerator>();
            _dustService = dustService ?? Locator.Current.GetService<DustService>();
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            var canLoadDataCommand = this.WhenAnyValue(p => p.SelectedStation)
                .Select(p => p != null);

            LoadDataCommand = ReactiveCommand.CreateFromObservable<Station, Record[]>(station =>
            {
                //Info = new DashboardInfo();
                _values.Clear();

                IObservable<Record[]> s = _dustService.GetAvailableParametersAsync(station)
                    .Select(@params => @params.Select(p => p.Param).ToArray())
                    .Select(@params => _dustService.GetStationRecordsAsync(station.Code, @params)).Switch();

                return Observable.Start(() => s, RxApp.TaskpoolScheduler).Switch().TakeUntil(CancelCommand); ;
            }, canLoadDataCommand);

            CancelCommand = ReactiveCommand.Create(
                () => { },
                this.LoadDataCommand.IsExecuting);

            this.WhenActivated(cleanup =>
            {
                var share = _values.Connect().Publish().RefCount();
                share.ToCollection().Select(records =>
                {
                    return Enum.GetValues(typeof(RecordType)).Cast<RecordType>()
                        .Select(type => records.LastOrDefault(r => r.Type == type));
                }).BindTo(this, vm=>vm.LastRecords).DisposeWith(cleanup);

                share.Bind(StationData).Subscribe().DisposeWith(cleanup);

                LoadDataCommand.ThrownExceptions.Subscribe(ShowError).DisposeWith(cleanup);

                LoadDataCommand.Subscribe(records =>
                {
                    _values.Edit(e => e.AddRange(records));
                }).DisposeWith(cleanup);

                var canSaveToExcelCommand = StationData.ObserveCollectionChanges().Select(_ => StationData.Count > 0);

                SaveToExcelCommand = ReactiveCommand.CreateFromTask<IEnumerable<Record>>(data => _excelGenerator.CreateExcel(SelectedStation.Code, data), canSaveToExcelCommand);

                SaveToExcelCommand.ThrownExceptions.Subscribe(ShowError);

                this.WhenAnyValue(p => p.SelectedStation).Where(p => p != null).Do(_ => CancelCommand.Execute().Subscribe()).InvokeCommand(LoadDataCommand).DisposeWith(cleanup); ;
            });
        }

        public ReactiveCommand<Unit, Unit> CancelCommand { get; set; }

        protected abstract void ShowError(Exception ex);

        public ReactiveCommand<IEnumerable<Record>, Unit> SaveToExcelCommand { get; set; }

        public ReactiveCommand<Station, Record[]> LoadDataCommand { get; set; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
        public string UrlPathSegment { get; } = "dashboard";
        public IScreen HostScreen { get; }
    }
}
