using System;
using System.Collections.Generic;
using System.Text;
using DynamicData.Binding;
using ThaiDust.Core.Model.Persistent;

namespace ThaiDust.Core
{
    public class Instance
    {
        public ObservableCollectionExtended<Station> SelectedStations { get; set; } = new ObservableCollectionExtended<Station>();
    }
}
