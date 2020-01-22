using System;
using System.Globalization;
using ReactiveUI.Fody.Helpers;

namespace ThaiDust.Core.Model.Persistent
{
    public class Record : Entity
    {
        [Reactive] public DateTime DateTime { get; set; }
        [Reactive] public string Param { get; set; }
        [Reactive] public double? Value { get; set; }

        public static string DateTimeBinding(DateTime v)
        {
            return v.ToString("dddd, dd MMMM yyyy hh:mm", CultureInfo.GetCultureInfo("th-TH"));
        }

        public static string DateBinding(DateTime v)
        {
            return v.ToString("dddd, dd MMMM yyyy", CultureInfo.GetCultureInfo("th-TH"));
        }

        public static string TimeBinding(DateTime v)
        {
            return v.ToString("hh:mm", CultureInfo.GetCultureInfo("th-TH"));
        }
    }
}