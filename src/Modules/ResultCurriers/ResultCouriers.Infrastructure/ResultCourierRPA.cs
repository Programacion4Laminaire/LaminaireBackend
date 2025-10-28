using Microsoft.Extensions.Configuration;
using ResultCouriers.Application.Dtos;
using ResultCouriers.Application.Interfaces;
using System.Net.Http.Json;



namespace ResultCouriers.Infrastructure
{
    public class ResultCourierRPA:IResultCourierRPA
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public ResultCourierRPA(HttpClient httpClient , IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<ResultCourierResponseRPA> SendCallAsync(SendResultCourierRPADto command)
            {
               
                var endpointUrl = _configuration["RPA:EndpointUrl"];
            if (string.IsNullOrEmpty(endpointUrl))
            {
                throw new InvalidOperationException("La clave 'RPA:EndpointUrl' no fue encontrada o está vacía en la configuración.");
            }

            using var response = await _httpClient.PostAsJsonAsync(endpointUrl, command);

     
            string jsonString = await response.Content.ReadAsStringAsync();

           
            Console.WriteLine($"JSON recibido (Data): {jsonString}");

            response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ResultCourierResponseRPA>();
            }
        }
    }

