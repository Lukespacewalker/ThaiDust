using System;
using System.Globalization;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ThaiDust.Models
{
    public class Station : ReactiveObject
    {
        [Reactive] public string Id { get; set; }
        [Reactive] public string Name { get; set; }
    }
    public class StationValue : ReactiveObject
    {
        [Reactive] public DateTime DateTime { get; set; }
        [Reactive] public double? Value { get; set; }

        public static string DateTimeBinding(DateTime v)
        {
            return v.ToString("dddd, dd MMMM yyyy",CultureInfo.GetCultureInfo("th-TH"));
        }
    }
}