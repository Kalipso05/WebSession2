using DataCenter.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebSessionSecond
{
    internal class TrackerService
    {
        private readonly HttpClient _httpClient;

        public TrackerService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<SecurityAccessLog>> GetTrackingDataAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:8080/api/track");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var trackingData = JsonConvert.DeserializeObject<List<SecurityAccessLog>>(jsonResponse);
                return trackingData;
            }
            else
            {
                throw new HttpRequestException($"Ошибка при получении данных: {response.ReasonPhrase}");
            }
        }
    }
}
