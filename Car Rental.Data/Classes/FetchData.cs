using Car_Rental.Common.Classes;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System.Net.Http.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Data.Classes;

public class FetchData
{
    private string errorMessage = "No errors encountered";
    private readonly HttpClient _httpClient;
    public FetchData(HttpClient httpClient) => _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public async Task<IEnumerable<T>?> FetchDataAsync<T>(string path, string filename)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<T>>($"{path}{filename}");
        }
        catch (Exception ex)
        {
            errorMessage = $"An error occurred while fetching {path}{filename}: {ex.Message}";
            return null;
        }
    }
    public string GetErrorMessage() => errorMessage;
}