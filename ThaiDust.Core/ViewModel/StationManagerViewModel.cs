using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using ThaiDust.Core.Extensions;
using ThaiDust.Core.Model.Persistent;
using ThaiDust.Core.Service;

namespace ThaiDust.Core.ViewModel
{
    public class StationManagerViewModel : ReactiveObject, IRoutableViewModel, IActivatableViewModel, IViewModelInfo
    {
        private DustService _dustService;
        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        private readonly SourceList<Station> _stationsSource = new SourceList<Station>();

        public string Title { get; }= "Manage stations";

        [Reactive] public IEnumerable<Station> SelectedAvailableStation { get; set; }
        [Reactive] public IEnumerable<Station> SelectedManagedStation { get; set; }

        public ObservableCollectionExtended<Station> AvailableStations = new ObservableCollectionExtended<Station>();
        public ObservableCollectionExtended<Station> ManagedStations => _dustService.ManagedStations2;

        public ReactiveCommand<IEnumerable<Station>, Unit> AddStationsCommand;
        public ReactiveCommand<IEnumerable<Station>, Unit> RemoveStationsCommand;

        public ReactiveCommand<Unit, Unit> SaveStationsCommand;

        public StationManagerViewModel(DustService dustService = null, IScreen screen = null)
        {
            _dustService = dustService ?? Locator.Current.GetService<DustService>();

            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            _stationsSource.AddRange(Core.Model.Stations.All);
            _stationsSource.Connect()
                .AutoRefreshOnObservable(_=>ManagedStations.GetCollectionChangedObservable())
                .Filter(s => !ManagedStations.Any(p => p.Code.Equals(s.Code, StringComparison.InvariantCultureIgnoreCase)))
                .Sort(SortExpressionComparer<Station>.Ascending(s=>s.Code))
                .Bind(AvailableStations)
                .Subscribe();

            AddStationsCommand = ReactiveCommand.Create<IEnumerable<Station>>(stations =>
            {
                foreach (var station in stations)
                {
                    if (ManagedStations.Contains(station)) continue;
                    ManagedStations.Add(station);
                }
            }, this.WhenAnyValue(p => p.SelectedAvailableStation).Where(p => p != null).Select(s => s.Any()));


            RemoveStationsCommand = ReactiveCommand.Create<IEnumerable<Station>>(stations =>
            {
                foreach (var station in stations)
                {
                    ManagedStations.Remove(station);
                }
            }, this.WhenAnyValue(p => p.SelectedManagedStation).Where(p => p != null).Select(s => s.Any()));

            SaveStationsCommand = ReactiveCommand.Create(_dustService.SaveManagedStationsToDatabase);
        }

        public string UrlPathSegment { get; } = "stationManager";
        public IScreen HostScreen { get; }
    }
}
