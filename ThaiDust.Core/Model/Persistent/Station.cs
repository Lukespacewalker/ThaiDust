using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI;

namespace ThaiDust.Core.Model.Persistent
{
    public class Station : ReactiveObject, IComparer<Station>
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        [ForeignKey("StationCode")]
        public virtual List<Record> Records { get; set; }

        public int Compare(Station x, Station y)
        {
            return string.Compare(x?.Code, y?.Code, StringComparison.Ordinal);
        }
    }
}