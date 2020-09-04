using System.Collections.Generic;

namespace KleinMapLibrary.Models
{
    public class ParamData
    {
        public string key { get; set; }
        public IEnumerable<Data> values { get; set; }
    }

    public class Data
    {
        public string date { get; set; }
        public double? value { get; set; }
    }
}
