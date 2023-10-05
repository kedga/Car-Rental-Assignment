using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace Car_Rental.Business.Classes;

public class DataManagement
{
    private string _errorMessage = string.Empty;
    public event Action DataChanged = delegate { };
    private readonly IData _dataAccess;
    private string _lastVehicleSortingOption = "RegistrationNumber";
    private string _vehicleSortingOption = "RegistrationNumber";
    private bool _vehicleSortingDescending = true;
    public DataManagement(IData dataAccess) => _dataAccess = dataAccess;
    public IData DataAccess => _dataAccess;
    public Dictionary<int, bool> ValidateAggressivelyDict { get; set; } = Enumerable.Range(1, 10).ToDictionary(x => x, x => false);
    public List<IVehicle> Vehicles { get; set; } = new();
    public List<IPerson> Customers { get; set; } = new();
    public List<IBooking> Bookings { get; set; } = new();
    public IEnumerable<T> GetDataObjectsOfType<T>() where T : class => _dataAccess.GetDataObjectsOfType<T>();
    public void ClearErrorMessage() => _errorMessage = string.Empty;
    public void AddDataObject(IDataObject dataObject) => _dataAccess.AddDataObject(dataObject);
    public async Task InitializeDataAsync()
    {
        await _dataAccess.FetchAndAddAsync<Sedan>("data/", "sedans.json");
        await _dataAccess.FetchAndAddAsync<Motorcycle>("data/", "motorcycles.json");
        await _dataAccess.FetchAndAddAsync<Van>("data/", "vans.json");
        await _dataAccess.FetchAndAddAsync<StationWagon>("data/", "stationwagons.json");
        await _dataAccess.FetchAndAddAsync<Booking>("data/", "bookings.json");
        await _dataAccess.FetchAndAddAsync<Person>("data/", "people.json");
    }
    public void RefreshData()
    {
        Bookings = GetDataObjectsOfType<IBooking>().ToList();
        int highestId = Bookings.Max(booking => booking.Id);
        foreach (var booking in Bookings.Where(booking => booking.Id == 0))
        {
            booking.Id = highestId + 1;
            highestId++;
        }
        Customers = GetDataObjectsOfType<IPerson>().ToList();
        Vehicles = GetDataObjectsOfType<IVehicle>().ToList();
        //SortObjects(_vehicleSortingOption, false);
        DataChanged?.Invoke();
    }
    //public void SortVehicles(VehicleSortingOption sortBy, bool reorder = true)
    //{
    //    _vehicleSortingOption = sortBy;
    //    var propertyName = sortBy.ToString();

    //    if (reorder)
    //    {
    //        if (_lastVehicleSortingOption == sortBy) // if same option clicked as last time
    //        {
    //            _vehicleSortingDescending = !_vehicleSortingDescending;
    //        }
    //        else // if new option is clicked
    //        {
    //            _lastVehicleSortingOption = sortBy;
    //            _vehicleSortingDescending = false;
    //        }
    //    }

    //    Vehicles = _vehicleSortingDescending ? 
    //        Vehicles.OrderByDescending(GetPropertyValueFunc(propertyName)).ToList() : 
    //        Vehicles.OrderBy(GetPropertyValueFunc(propertyName)).ToList();
    //}
    public void SortObjects(Dictionary<string, Func<IVehicle, object>> properyDict, string propertyName, bool reorder = true)
    {
        if (reorder)
        {
            if (_lastVehicleSortingOption == propertyName) // if the same property is clicked as last time
            {
                _vehicleSortingDescending = !_vehicleSortingDescending;
            }
            else // if a new property is clicked
            {
                _lastVehicleSortingOption = propertyName;
                _vehicleSortingDescending = false;
            }
        }

        if (properyDict.TryGetValue(propertyName, out var orderByExpression))
        {
            Vehicles = _vehicleSortingDescending ?
                Vehicles.OrderByDescending(orderByExpression).ToList() :
                Vehicles.OrderBy(orderByExpression).ToList();
        }
        else
        {
            // Handle the case when an unsupported property is selected
            // You can use a default sorting property or display an error message.
        }
    }

    //PropertyInfo propertyInfo
    private static Func<IVehicle, object> GetPropertyValueFunc(string propertyName)
    {
        var param = Expression.Parameter(typeof(IVehicle), "x");
        var property = Expression.Property(param, propertyName);
        var conversion = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<IVehicle, object>>(conversion, param).Compile();
    }

    public void SetErrorMessage(string message)
        {
            _errorMessage = message;
            DataChanged?.Invoke();
        }
        public string GetErrorMessage() => _errorMessage;

}
