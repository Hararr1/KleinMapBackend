using KleinMapAPI.API;
using KleinMapAPI.Models;
using KleinMapAPI.Workspace;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KleinMapAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GIOSController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<Station>> GetStations(int Id)
        {
            DateTime time = WorkspaceItemCollection.DownloadTime;

            string province;
            DictonaryValues.Provinces.TryGetValue(Id, out province);

            if (WorkspaceItemCollection.AllStations == null ||
                time.AddMinutes(30) < DateTime.Now)
            {
                WorkspaceItemCollection.AllStations = await APIClient.Instance.GetAllStations();
                WorkspaceItemCollection.DownloadTime = DateTime.Now;
            }

            var output =  WorkspaceItemCollection.AllStations.Where(x => x.city.commune.provinceName == province);
            return output;
        }
    }
}