using KleinMapLibrary.Enums;
using System;
using System.Collections.Generic;

namespace KleinMapLibrary.Models
{
    public class Station
    {
        public int id { get; set; }
        public string stationName { get; set; }
        public string gegrLat { get; set; }
        public string gegrLon { get; set; }
        public City city { get; set; }
        public string? addressStreet { get; set; }

        public IEnumerable<Sensor> sensors { get; set; }
        public State state { get; set; }
        public DateTime lastUpdate { get; set; }
    }
}
