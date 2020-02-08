using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ThaiDust.Core.Model
{
    public class StationParam : ReactiveObject
    {
        [Reactive] public string Param { get; set; }
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