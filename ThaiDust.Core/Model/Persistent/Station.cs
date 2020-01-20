using System.Collections.Generic;

namespace ThaiDust.Core.Model.Persistent
{
    public class Station : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public List<Record> Records { get; set; }
    }
}