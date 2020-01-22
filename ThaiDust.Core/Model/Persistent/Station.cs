using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ReactiveUI;

namespace ThaiDust.Core.Model.Persistent
{
    public class Station : ReactiveObject
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

        public List<Record> Records { get; set; }
    }
}