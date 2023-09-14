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

namespace Car_Rental.Data.Classes
{
    public class CollectionData : IData
    {
        private Dictionary<Type, IEnumerable<object>?> _dataObjects = new Dictionary<Type, IEnumerable<object>?>();
        private string _errorMessage = "No errors encountered";
        private readonly HttpClient _httpClient;

        public CollectionData(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task InitializeDataAsync()
        {
            _dataObjects[typeof(IVehicle)] = await FetchDataAsync<Vehicle>("data/", "vehicles.json");
            _dataObjects[typeof(IBooking)] = await FetchDataAsync<Booking>("data/", "bookings.json");
            _dataObjects[typeof(IPerson)] = await FetchDataAsync<Person>("data/", "people.json");
        }

        public IEnumerable<T> GetDataObjectsOfType<T>()
        {
            if (_dataObjects.TryGetValue(typeof(T), out var data))
            {
                return data.Cast<T>();
            }
            else
            {
                return Enumerable.Empty<T>();
            }
        }

        async Task<IEnumerable<T>?> FetchDataAsync<T>(string path, string filename)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<IEnumerable<T>?>($"{path}{filename}");
            }
            catch (Exception ex)
            {
                _errorMessage += $"\nAn error occurred while fetching {path}{filename}: {ex.Message}";
                return Enumerable.Empty<T>();
            }
        }
        public void AddDataObject(IDataObject dataObject)
        {
            if (dataObject == null)
            {
                _errorMessage = $"ArgumentNullException {nameof(dataObject)}";
                throw new ArgumentNullException(nameof(dataObject));
            }

            Type objectType = dataObject.GetType();

            if (_dataObjects.TryGetValue(objectType, out var dataList))
            {
                if (dataList == null)
                {
                    _errorMessage = "Create a new list for this type if it doesn't exist";
                    dataList = new List<object>();
                    _dataObjects[objectType] = dataList;
                }

                _errorMessage = "Add the object to the list; success";
                dataList.Cast<object>().ToList().Add(dataObject);
            }
            else
            {
                _errorMessage = $"Type {objectType} not found in _dataObjects, so create a new list for this type";
                var newList = new List<object> { dataObject };
                _dataObjects.Add(objectType, newList);
            }
        }


        public string GetErrorMessage() => _errorMessage;
    }
}
