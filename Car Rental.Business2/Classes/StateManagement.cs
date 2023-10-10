using Car_Rental.Common.Classes;
using Car_Rental.Common.Interfaces;
using Car_Rental.Common.Utilities;
using Car_Rental.Data.Classes;
using Car_Rental.Data.Interfaces;

namespace Car_Rental.Business.Classes;

public class StateManagement
{
    public event Action DataChanged = delegate { };
    private string _errorMessage = string.Empty;
    private readonly IData _dataAccess;
    public StateManagement(IData dataAccess)
    {
        _dataAccess = dataAccess;
    }
	public IData DataAccess => _dataAccess;
    public Dictionary<int, bool> ValidateAggressivelyDict { get; set; } = Enumerable.Range(1, 10).ToDictionary(x => x, x => false);
    public List<IVehicle> Vehicles { get; set; } = new();
    public List<IPerson> Customers { get; set; } = new();
    public List<IBooking> Bookings { get; set; } = new();
    public async Task<IEnumerable<T>> GetDataObjectsOfType<T>() where T : class => await _dataAccess.GetDataObjectsOfType<T>();
    public void AddDataObject(IDataObject dataObject) => _dataAccess.AddDataObject(dataObject);
    public async Task InitializeDataAsync()
    {
        await _dataAccess.FetchAndAddAsync<Sedan>("data/", "sedans.json");
        await _dataAccess.FetchAndAddAsync<Motorcycle>("data/", "motorcycles.json");
        await _dataAccess.FetchAndAddAsync<Van>("data/", "vans.json");
        await _dataAccess.FetchAndAddAsync<StationWagon>("data/", "stationwagons.json");
        await _dataAccess.FetchAndAddAsync<Booking>("data/", "bookings.json");
        await _dataAccess.FetchAndAddAsync<Person>("data/", "people.json");

        DataMatching.MatchAndInsert(
            await _dataAccess.GetDataObjectsOfType<IBooking>(),
            await _dataAccess.GetDataObjectsOfType<IVehicle>(),
            outerEntity => outerEntity.RegistrationNumber,
            innerEntity => innerEntity.RegistrationNumber,
            (booking, vehicle) => booking.Vehicle = vehicle);
        DataMatching.MatchAndInsert(
            await _dataAccess.GetDataObjectsOfType<IBooking>(),
            await _dataAccess.GetDataObjectsOfType<IPerson>(),
            outerEntity => outerEntity.CustomerSsn,
            innerEntity => innerEntity.SocialSecurityNumber,
            (booking, customer) => booking.Customer = customer);
    }
    public async Task RefreshData()
    {
        var bookingsAsync = await GetDataObjectsOfType<IBooking>();
        Bookings = bookingsAsync.ToList();
        int highestId = Bookings.Max(booking => booking.Id);
        foreach (var booking in Bookings.Where(booking => booking.Id == 0))
        {
            booking.Id = highestId + 1;
            highestId++;
        }

        var customersAsync = await GetDataObjectsOfType<IPerson>();
        Customers = customersAsync.ToList();

        var vehiclesAsync = await GetDataObjectsOfType<IVehicle>();
        Vehicles = vehiclesAsync.ToList();

		DataChanged?.Invoke();
    }
    public void SetErrorMessage(string message)
    {
        _errorMessage = message;
        DataChanged?.Invoke();
    }
    public string GetErrorMessage() => _errorMessage;
    public void ClearErrorMessage()
    {
        _errorMessage = string.Empty;

        foreach (var key in ValidateAggressivelyDict.Keys.ToList())
        {
            ValidateAggressivelyDict[key] = false;
        }

        DataChanged?.Invoke();
    }

}
