using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ThaiDust.Core.Model.Persistent;

namespace ThaiDust.Core.Model
{
    public class StationParam : ReactiveObject
    {
        [Reactive] public RecordType Param { get; set; }
        [Reactive] public string Name { get; set; }
    }

    public class DashboardInfo : ReactiveObject
    {
        [Reactive] public string PM25 { get; set; } = "--";
        [Reactive] public string PM10 { get; set; } = "--";
        [Reactive] public string O3 { get; set; } = "--";
        [Reactive] public string NO2 { get; set; } = "--";
        [Reactive] public string CO { get; set; } = "--";
        [Reactive] public string SO2 { get; set; } = "--";
        [Reactive] public DateTime CurrentDateTime { get; set; }
    }
}