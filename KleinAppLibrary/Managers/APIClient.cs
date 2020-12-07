using KleinMapLibrary.Enums;
using KleinMapLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace KleinMapLibrary.Managers
{
    public class APIClient
    {
        // ------ SINGLETON ------ //
        private static APIClient instance;
        public static APIClient Instance => instance = instance ?? new APIClient();

        // ------ PROPERTIES ------ //
        private HttpClient apiClient;
        private string baseUrl;

        // ------ CTOR AND INIT METHOD ------ //
        private APIClient()
        {
            this.baseUrl = "http://api.gios.gov.pl/pjp-api/rest/";
            InitializeClient();
        }

        private void InitializeClient()
        {
            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(this.baseUrl);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.Timeout = TimeSpan.FromSeconds(30);
        }

        // ------ API CALLS ------ //
        public async Task<IEnumerable<Station>> GetAllStations()
        {
            using (HttpResponseMessage response = await apiClient.GetAsync($"station/findAll"))
            {
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        IEnumerable<Station> stations = await response.Content.ReadAsAsync<IEnumerable<Station>>();

                        foreach (Station station in stations)
                        {
                            station.lastUpdate = DateTime.Now;
                        }

                        return stations;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }

                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
        }
        public async Task<IEnumerable<Sensor>> GetSensors(int stationId)
        {
            using (HttpResponseMessage response = await apiClient.GetAsync($"station/sensors/{stationId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        IEnumerable<Sensor> sensors = await response.Content.ReadAsAsync<IEnumerable<Sensor>>();

                        // Get data from sensors
                        if (sensors != null)
                        {
                            foreach(Sensor sensor in sensors)
                            {
                                sensor.lastUpdate = DateTime.Now;
                                sensor.data = await GetSensorData(sensor.id);

                                if (sensor.data.values != null && sensor.data.values.Count() > 0)
                                {
                                    var x = sensor.data.values.Max(x => x == null ? 0 : x.value);
                                    sensor.worstValue = x.Value;
                                    sensor.type = sensor.data.key != null ? PrepareDataManager.SetType(sensor.data.key) : ParamType.Unknown;

                                    // CHECK FIRST 4 ELEMENTS
                                    Data lastData = sensor.data.values.ElementAtOrDefault(0);
                                    lastData = lastData.value == null ? sensor.data.values.ElementAtOrDefault(1) : lastData;
                                    lastData = lastData.value == null ? sensor.data.values.ElementAtOrDefault(2) : lastData;
                                    lastData = lastData.value == null ? sensor.data.values.ElementAtOrDefault(3) : lastData;

                                    double lastValue = lastData.value ?? double.MaxValue;

                                    if (lastValue != double.MaxValue)
                                    {
                                        sensor.state = PrepareDataManager.SetState(sensor.type, lastValue);
                                        sensor.currentValue = lastValue;
                                    }
                                    else
                                    {
                                        sensor.state = State.Unknown;
                                    }
                                }
                            }
                                   
                        }

                        return sensors;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
        }
        public async Task<ParamData> GetSensorData(int sensorId)
        {
            using (HttpResponseMessage response = await apiClient.GetAsync($"data/getData/{sensorId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var paramData = await response.Content.ReadAsAsync<ParamData>();

                        if (paramData is ParamData)
                        {
                            return paramData as ParamData;
                        }
                        else
                        {
                            return new ParamData();
                        }
                    }
                    catch (Exception)
                    {
                        return new ParamData();
                    }
                }
                else
                {
                    return new ParamData();
                }
            }
        }
    }
}
