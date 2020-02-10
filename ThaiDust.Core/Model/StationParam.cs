using System;
using System.Collections.Generic;
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

    //public class DashboardInfo : ReactiveObject
    //{
    //    [Reactive]
    //    public Dictionary<string, Record> LastestRecords { get; set; } = new Dictionary<string, Record>
    //    {
    //        {"PM25",null },{"PM10",null},{"O3",null},{"SO0",null},{"CO",null},{"NO",null}
    //    };
    //}
}