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
        [Reactive] public double? PM25 { get; set; }
        [Reactive] public double? PM10 { get; set; }
        [Reactive] public double? O3 { get; set; }
        [Reactive] public double? NO2 { get; set; }
        [Reactive] public double? CO { get; set; }
        [Reactive] public double? SO2 { get; set; }
        [Reactive] public DateTime CurrentDateTime { get; set; }
    }
}