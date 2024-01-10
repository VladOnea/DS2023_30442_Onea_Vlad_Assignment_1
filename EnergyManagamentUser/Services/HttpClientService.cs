using System.Net;
using System.Text.Json;
using System.Text;
using EnergyManagamentUser.Dtos;

namespace EnergyManagamentUser.Services
{
    public static class HttpClientService
    {
        public static async Task<HttpStatusCode> PostAsync(HttpClient httpClient, string url,UserDeviceDto body)
        {
            var address = new Uri(url);

            using StringContent jsonContent = new(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");

            using var responseMessage = await httpClient.PostAsync(address, jsonContent);

            return responseMessage.StatusCode;
        }

        public static async Task<HttpStatusCode> DeleteAsync(HttpClient httpClient, string url)
        {
            using var responseMessage = await httpClient.DeleteAsync(url);

            return responseMessage.StatusCode;
        }
    }
}
