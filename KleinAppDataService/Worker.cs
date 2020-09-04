using KleinAppLibrary.Managers;
using KleinAppLibrary.Models;
using KleinAppLibrary.Values;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KleinAppDataService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration configuration;
        private static IEnumerable<Station> AllStations { get; set; }


        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            _logger = logger;
            configuration = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                 string dataPath = configuration.GetSection("DataDirectory").Value;
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                 AllStations = await APIClient.Instance.GetAllStations();
                _logger.LogInformation("Get all station at: {time}", DateTimeOffset.Now);

                foreach (Station station in AllStations)
                {
                    station.sensors = await APIClient.Instance.GetSensors(station.id);
                    _logger.LogInformation("Get sensors for {station} with data at: {time}", new object[] { station.stationName, DateTimeOffset.Now });
                    station.state = station.sensors.Max(sensor => sensor.state);
                }

                foreach (var province in DictonaryValues.Provinces)
                {
                    IEnumerable<Station> stations = AllStations.Where(station => station.city.commune.provinceName == province.Value);  
                    await FileManager.Instance.SaveDataAsync(stations, province.Value, dataPath);
                    _logger.LogInformation("Save data in province {province} at: {time}", new object[] { province.Value, DateTimeOffset.Now });
                }

                await Task.Delay(1800000, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The service has been stopped...");
            return base.StopAsync(cancellationToken);
        }
    }
}
