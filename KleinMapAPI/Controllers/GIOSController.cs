using KleinAppLibrary.Managers;
using KleinAppLibrary.Models;
using KleinAppLibrary.Values;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KleinMapAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GIOSController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public GIOSController(IConfiguration config)
        {
            configuration = config;
        }

        [HttpGet]
        public async Task<IEnumerable<Station>> GetStations(int provinceId)
        {
            string province;
            DictonaryValues.Provinces.TryGetValue(provinceId, out province);

            string dataPath = configuration.GetSection("DataDirectory").Value;
            return await FileManager.Instance.LoadDataAsync(province, dataPath);
        }
    }
}