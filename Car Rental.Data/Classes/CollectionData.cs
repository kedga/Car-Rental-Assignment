using Car_Rental.Common.Classes;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Car_Rental.Data.Classes;

public class CollectionData : IData
{
    private readonly IFetchData _fetchData;
    public CollectionData(IFetchData fetchData)
    {
        _fetchData = fetchData;
    }

    private List<IPerson> _people = new();
    private List<IVehicle> _vehicles = new();
    private List<IBooking> _bookings = new();

    public List<T> Get<T>(Func<T, bool>? filter = null) where T : class
    {
        var collectionField = GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(f => f.FieldType == typeof(List<T>));

        if (collectionField == null)
        {
            throw new InvalidOperationException($"Unsupported type: {typeof(T)}");
        }

        var collection = (collectionField.GetValue(this) as IEnumerable<T>) ?? throw new InvalidDataException();

        if (filter == null)
        {
            return collection.ToList();
        }

        return collection.Where(filter).ToList();
    }

    public T? Single<T>(Func<T, bool>? filter = null) where T : class
    {
        var collection = Get(filter);
        if (filter is null)
        {
            return collection.SingleOrDefault();
        }
        return collection.SingleOrDefault(filter);
    }

    public void Add<T>(T item) where T : class
    {
        var collectionField = GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(f => f.FieldType == typeof(List<T>));

        if (collectionField == null)
        {
            throw new InvalidOperationException($"Unsupported type: {typeof(T)}");
        }

        var collection = (collectionField.GetValue(this) as List<T>) ?? throw new InvalidDataException();

        collection.Add(item);
    }

    public void Remove<T>(Func<T, bool> predicate) where T : class
    {
        var collectionField = GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(f => f.FieldType == typeof(List<T>));

        if (collectionField == null)
        {
            throw new InvalidOperationException($"Unsupported type: {typeof(T)}");
        }

        var collection = (collectionField.GetValue(this) as List<T>) ?? throw new InvalidDataException();

        collection.RemoveAll(new Predicate<T>(predicate));
    }

    async Task<List<T>> FetchAndAddAsync<T>(string path, string filename) where T : IDataObject
    {
        var data = await _fetchData.FetchDataAsync<T>(path, filename);

        if (data is null) throw new InvalidOperationException(path + filename + "was not able to be fetched");

        return data;
    }
    public async Task Initialize()
    {
        var basePath = "data/";
        _vehicles.AddRange(await FetchAndAddAsync<Sedan>(basePath, "sedans.json"));

        _vehicles.AddRange(await FetchAndAddAsync<Motorcycle>(basePath, "motorcycles.json"));

        _vehicles.AddRange(await FetchAndAddAsync<Van>(basePath, "vans.json"));

        _vehicles.AddRange(await FetchAndAddAsync<StationWagon>(basePath, "stationwagons.json"));

        _bookings.AddRange(await FetchAndAddAsync<Booking>(basePath, "bookings.json"));

        _people.AddRange(await FetchAndAddAsync<Person>(basePath, "people.json"));

        foreach (var booking in _bookings)
        {
            booking.Id = _bookings.Max(x => x.Id) + 1;
        }

            DataMatching.MatchAndInsert(
            Get<IBooking>(null),
            Get<IVehicle>(null),
            outerEntity => outerEntity.RegistrationNumber,
            innerEntity => innerEntity.RegistrationNumber,
            (booking, vehicle) => booking.Vehicle = vehicle);
        DataMatching.MatchAndInsert(
            Get<IBooking>(null),
            Get<IPerson>(null),
            outerEntity => outerEntity.CustomerSsn,
            innerEntity => innerEntity.SocialSecurityNumber,
            (booking, customer) => booking.Customer = customer);

    }
}
