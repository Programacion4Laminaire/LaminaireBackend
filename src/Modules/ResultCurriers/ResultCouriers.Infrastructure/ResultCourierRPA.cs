using ResultCouriers.Application.Dtos;
using ResultCouriers.Application.Interfaces;
using System.Net.Http.Json;



namespace ResultCouriers.Infrastructure
{
    public class ResultCourierRPA:IResultCourierRPA
    {
        private readonly HttpClient _httpClient;
        public ResultCourierRPA(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResultCourierResponseRPA> SendCallAsync(SendResultCourierRPADto command)
            {
               
                var endpointUrl = "http://localhost:3000/ejecutar-comando";

                using var response = await _httpClient.PostAsJsonAsync(endpointUrl, command);

     
            string jsonString = await response.Content.ReadAsStringAsync();

           
            Console.WriteLine($"JSON recibido (Data): {jsonString}");

            response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ResultCourierResponseRPA>();
            }
        }
    }

