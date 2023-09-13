using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading.Tasks;
using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;


namespace Car_Rental.Data.Classes;

public class CollectionData : IData
{
    private string errorMessage = "No errors encountered";
    private readonly HttpClient _httpClient;
    private const string VehiclesJsonPath = "data/vehicles.json";
    private const string BookingsJsonPath = "data/bookings.json";
    private const string PeopleJsonPath = "data/people.json";

    public CollectionData(HttpClient httpClient) => _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public async Task<IEnumerable<IVehicle>> GetVehiclesAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Vehicle>>(VehiclesJsonPath);
        }
        catch (Exception ex)
        {
            errorMessage = $"An error occurred while fetching vehicles: {ex.Message}";
            return null;
        }
    }

    public async Task<IEnumerable<IBooking>> GetBookingsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Booking>>(BookingsJsonPath);
        }
        catch (Exception ex)
        {
            errorMessage = $"An error occurred while fetching bookings: {ex.Message}";
            return null;
        }
    }

    public async Task<IEnumerable<IPerson>> GetPeopleAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Person>>(PeopleJsonPath);
        }
        catch (Exception ex)
        {
            errorMessage = $"An error occurred while fetching persons: {ex.Message}";
            return null;
        }
    }

    public string GetErrorMessage() => errorMessage;
}