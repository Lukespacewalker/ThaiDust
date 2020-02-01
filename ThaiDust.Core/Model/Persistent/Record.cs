using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using ReactiveUI.Fody.Helpers;

namespace ThaiDust.Core.Model.Persistent
{
    public enum RecordType
    {
        PM25,
        PM10,
        SO2,
        O3,
        CO,
        NO2,
    }

    public class Record : Entity
    {
        [Reactive] public DateTime DateTime { get; set; }
        [Reactive] public RecordType Type { get; set; }
        [Reactive] public double? Value { get; set; }

        public virtual Station Station { get; set; }
        [ForeignKey(nameof(Station))]
        public string StationCode { get; set; }

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