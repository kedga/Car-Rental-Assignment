using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;

namespace Car_Rental.Data.Classes;

public class CollectionData : IData
{
    private Dictionary<Type, List<object>> _dataObjects = new Dictionary<Type, List<object>>();
    private string _errorMessage = "No errors encountered";
    private readonly HttpClient _httpClient;

    public CollectionData(HttpClient httpClient) => _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public async Task InitializeDataAsync()
    {
        await Console.Out.WriteLineAsync("CollectionData.InitializeDataAsync started");
        ProcessDataAndAddToDictionary(await FetchDataAsync<Vehicle>("data/", "vehicles.json"));
        ProcessDataAndAddToDictionary(await FetchDataAsync<Booking>("data/", "bookings.json"));
        ProcessDataAndAddToDictionary(await FetchDataAsync<Person>("data/", "people.json"));
    }
    private void ProcessDataAndAddToDictionary<T>(IEnumerable<T> data) where T : IDataObject
    {
        foreach (var item in data)
        {
            AddDataObject(item);
            //Console.WriteLine(item);
        }
    }

    async Task<List<T>> FetchDataAsync<T>(string path, string filename)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<T>>($"{path}{filename}");
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public IEnumerable<T> GetDataObjectsOfType<T>() =>_dataObjects.TryGetValue(typeof(T), out var data) ? data!.Cast<T>() : Enumerable.Empty<T>();

    public void AddDataObject(IDataObject dataObject)
    {
        if (dataObject == null)
            throw new ArgumentNullException(nameof(dataObject));

        Type objectType = dataObject.GetType();

        foreach (var interfaceType in objectType.GetInterfaces())
        {
            if (!_dataObjects.ContainsKey(interfaceType)) _dataObjects[interfaceType] = new List<object>();

            _dataObjects[interfaceType].Add(dataObject);
        }

        if (!_dataObjects.ContainsKey(objectType)) _dataObjects[objectType] = new List<object>();

        _dataObjects[objectType].Add(dataObject);
    }

    public void PrintDataObjects()
    {
        Console.WriteLine("Contents of Data Objects:");

        foreach (var kvp in _dataObjects)
        {
            Type key = kvp.Key;
            IEnumerable<object> values = kvp.Value ?? Enumerable.Empty<object>();

            Console.WriteLine($"Type: {key.FullName}");
            Console.WriteLine("Values:");

            foreach (var value in values)
            {
                Console.WriteLine($"  {value}");
            }

            Console.WriteLine();
        }
    }



    public string GetErrorMessage() => _errorMessage;
}
