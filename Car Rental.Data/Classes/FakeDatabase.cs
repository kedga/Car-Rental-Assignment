using Car_Rental.Common.Classes;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;

namespace Car_Rental.Data.Classes;

public class FakeDatabase
{
    public List<IVehicle> Vehicles { get; set; } = new();
    public List<IPerson> Customers { get; set; } = new();
    public List<IBooking> Bookings { get; set; } = new();

    private readonly IFetchData _fetchData;
    public FakeDatabase(IFetchData fetchData)
    {
        _fetchData = fetchData;
    }
    private readonly Dictionary<Type, List<object>> _data = new();
    public IEnumerable<T> GetDataObjectsOfType<T>()
    {
        return _data.TryGetValue(typeof(T), out var dataList) ? dataList!.Cast<T>() : Enumerable.Empty<T>();
    }

    public Task Add(IDataObject dataObject)
    {
        if (dataObject == null) throw new ArgumentNullException(nameof(dataObject));

        Type objectType = dataObject.GetType();

        foreach (var interfaceType in objectType.GetInterfaces())
        {
            if (!_data.ContainsKey(interfaceType)) _data[interfaceType] = new List<object>();

            _data[interfaceType].Add(dataObject);
        }

        if (!_data.ContainsKey(objectType)) _data[objectType] = new List<object>();

        _data[objectType].Add(dataObject);
        return Task.CompletedTask;
    }

    async Task FetchAndAddAsync<T>(string path, string filename) where T : IDataObject
    {
        IEnumerable<T>? data = await _fetchData.FetchDataAsync<T>(path, filename);

        if (data is not null)
        {
            foreach (var item in data)
            {
                if (item is not null)
                {
                    await Add(item);
                }
            }
        }
    }
    public void PrintDataObjects()
    {
        throw new NotImplementedException();
    }
    public async Task Initialize()
    {
        var basePath = "data/";
        await FetchAndAddAsync<Sedan>(basePath, "sedans.json");
        await FetchAndAddAsync<Motorcycle>(basePath, "motorcycles.json");
        await FetchAndAddAsync<Van>(basePath, "vans.json");
        await FetchAndAddAsync<StationWagon>(basePath, "stationwagons.json");
        await FetchAndAddAsync<Booking>(basePath, "bookings.json");
        await FetchAndAddAsync<Person>(basePath, "people.json");

        DataMatching.MatchAndInsert(
            GetDataObjectsOfType<IBooking>(),
            GetDataObjectsOfType<IVehicle>(),
            outerEntity => outerEntity.RegistrationNumber,
            innerEntity => innerEntity.RegistrationNumber,
            (booking, vehicle) => booking.Vehicle = vehicle);
        DataMatching.MatchAndInsert(
            GetDataObjectsOfType<IBooking>(),
            GetDataObjectsOfType<IPerson>(),
            outerEntity => outerEntity.CustomerSsn,
            innerEntity => innerEntity.SocialSecurityNumber,
            (booking, customer) => booking.Customer = customer);
    }
    public void RefreshData()
    {
        var bookingsAsync = GetDataObjectsOfType<IBooking>();
        Bookings = bookingsAsync.ToList();
        int highestId = Bookings.Max(booking => booking.Id);
        foreach (var booking in Bookings.Where(booking => booking.Id == 0))
        {
            booking.Id = highestId + 1;
            highestId++;
        }

        var customersAsync = GetDataObjectsOfType<IPerson>();
        Customers = customersAsync.ToList();

        var vehiclesAsync = GetDataObjectsOfType<IVehicle>();
        Vehicles = vehiclesAsync.ToList();
    }
}
