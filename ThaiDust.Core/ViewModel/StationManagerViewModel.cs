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
using ThaiDust.Core.Model.Persistent;

namespace ThaiDust.Core.ViewModel
{
    public class StationManagerViewModel : ReactiveObject, IRoutableViewModel, IActivatableViewModel
    {
        private readonly Instance _instance;
        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        private readonly SourceList<Station> _availableStations = new SourceList<Station>();

        [Reactive] public IEnumerable<Station> SelectedStations2 { get; set; }

        public ObservableCollectionExtended<Station> AvailableStations = new ObservableCollectionExtended<Station>();
        public ObservableCollectionExtended<Station> SelectedStations => _instance.SelectedStations;

        public ReactiveCommand<IEnumerable<Station>, Unit> AddStationsCommand;

        public StationManagerViewModel(Instance instance = null, IScreen screen = null)
        {
            _instance = instance ?? Locator.Current.GetService<Instance>();
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            _availableStations.AddRange(Core.Model.Stations.All);
            _availableStations.Connect()
                .Filter(s => !SelectedStations.Any(p => p.Code.Equals(s.Code, StringComparison.InvariantCultureIgnoreCase)))
                .Bind(AvailableStations)
                .Subscribe();

            AddStationsCommand = ReactiveCommand.Create<IEnumerable<Station>>(stations =>
            {
                
                foreach (var station in stations)
                {
                    if(SelectedStations.Contains(station)) continue;
                    SelectedStations.Add(station);
                    _availableStations.Remove(station);
                }
            }, this.WhenAnyValue(p => p.SelectedStations2).Where(p=>p!=null).Select(s => s.Count() > 0));
        }

        public string UrlPathSegment { get; } = "stationManager";
        public IScreen HostScreen { get; }
    }
}
