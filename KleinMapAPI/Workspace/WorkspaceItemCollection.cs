using KleinMapAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KleinMapAPI.Workspace
{
    public static  class WorkspaceItemCollection
    {
        public static DateTime DownloadTime { get; set; }
        public static IEnumerable<Station> AllStations { get; set; }

    }
}
