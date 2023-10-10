//using Car_Rental.Common.Classes;
//using Car_Rental.Common.Interfaces;
//using Car_Rental.Data.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Net.Http.Json;
//using System.Threading.Tasks;

//namespace Car_Rental.Data.Classes;

//public class JsonDatabase : IData
//{
//    private readonly IFetchData _fetchData;

//    private List<IDataObject> _dataObjects = new();
//    private List<Booking> _bookings = new();
//    private List<Person> _people = new();
//    private List<Motorcycle> _motorcycles = new();
//    private List<Sedan> _sedans = new();
//    private List<StationWagon> _stationWagons = new();
//    private List<Van> _vans = new();
//    public JsonDatabase(IFetchData fetchData)
//    {
//        _fetchData = fetchData;
//    }
//    public async Task AddDataObject(IDataObject dataObject)
//    {
//        _dataObjects.Add(dataObject);

//    }

//    public async Task FetchAndAddAsync<T>(string path, string filename) where T : IDataObject
//    {
//        IEnumerable<T>? data = await _fetchData.FetchDataAsync<T>(path, filename);

//        if (data != null)
//        {
//            foreach (var item in data)
//            {
//                if (item != null)
//                {
//                    switch (item)
//                    {
//                        case Booking bookingItem:
//                            _bookings.Add(bookingItem);
//                            break;
//                        case Motorcycle motorcycle:
//                            _motorcycles.Add(motorcycle);
//                            break;
//                        case Person person:
//                            _people.Add(person);
//                            break;
//                        case Sedan sedan:
//                            _sedans.Add(sedan);
//                            break;
//                        case StationWagon stationWagon:
//                            _stationWagons.Add(stationWagon);
//                            break;
//                        case Van van:
//                            _vans.Add(van);
//                            break;
//                    }
//                }
//            }
//            try
//            {
//                var jsonData = JsonSerializer.Serialize(_bookings);
//                var folder = "data/generated/";
//                Directory.CreateDirectory(folder);
//                var filePath = Path.Combine(folder, "bookings.json");
//                await File.WriteAllTextAsync(filePath, jsonData);
//                using FileStream createStream = File.Create(filePath);
//                await JsonSerializer.SerializeAsync(createStream, jsonData);
//                await createStream.DisposeAsync();
//                await Console.Out.WriteLineAsync($"File written successfully: {filePath}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"An error occurred: {ex.Message}");
//            }
//        }
//    }

//    public Task<IEnumerable<T>> GetDataObjectsOfType<T>()
//    {
//        throw new NotImplementedException();
//    }

//    public void PrintDataObjects()
//    {
//        throw new NotImplementedException();
//    }
//}
