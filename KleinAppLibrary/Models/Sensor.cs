using KleinAppLibrary.Enums;
using System;

namespace KleinAppLibrary.Models
{
    public class Sensor
    {
        public int id { get; set; }
        public int stationId { get; set; }
        public Parameters param { get; set; }
        public DateTime lastUpdate { get; set; }
        public ParamData data { get; set; }
        public ParamType type { get; set; }
        public State state { get; set; }
        public double worstValue { get; set; }
    }
}
