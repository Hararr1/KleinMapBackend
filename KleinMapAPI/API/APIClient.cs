using KleinMapAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace KleinMapAPI.API
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

        public APIClient()
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
                        return await response.Content.ReadAsAsync<IEnumerable<Station>>();
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

    }
}
