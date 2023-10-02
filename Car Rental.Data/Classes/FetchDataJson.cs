using Car_Rental.Data.Interfaces;
using System.Net.Http.Json;

namespace Car_Rental.Data.Classes;

public class FetchDataJson : IFetchData
{
    private readonly HttpClient _httpClient;
    public FetchDataJson(HttpClient httpClient) => _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public async Task<List<T>?> FetchDataAsync<T>(string path, string filename)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<T>>($"{path}{filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An exception occurred while fetching data: {ex}");
            return null;
        }
    }
}