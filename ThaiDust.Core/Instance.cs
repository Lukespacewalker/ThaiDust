using System;
using System.Collections.Generic;
using System.Text;
using DynamicData;
using DynamicData.Binding;
using Splat;
using ThaiDust.Core.Model.Persistent;
using ThaiDust.Core.Service;

namespace ThaiDust.Core
{
    public class Instance
    {
        private readonly DustDataService _dustDataService;
        private SourceList<Station> _managedStations = new SourceList<Station>();

        public Instance(DustDataService dustDataService = null)
        {
            _dustDataService = dustDataService ?? Locator.Current.GetService<DustDataService>();

            // Load all stations data from local database

        }

        public ObservableCollectionExtended<Station> SelectedStations { get; set; } = new ObservableCollectionExtended<Station>();
    }
}
