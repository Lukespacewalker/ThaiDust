using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI;

namespace ThaiDust.Core.Model.Persistent
{
    public class Station : ReactiveObject, IComparable<Station>
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        [ForeignKey("StationCode")]
        public virtual List<Record> Records { get; set;} = new List<Record>();

        public int CompareTo(Station other)
        {
            return string.Compare(this.Code, other.Code, StringComparison.Ordinal);
        }
    }
}