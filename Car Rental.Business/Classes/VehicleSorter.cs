using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;

namespace Car_Rental.Business.Classes;

public class VehicleSorter
{
	public List<IVehicle> SortedVehicles = new();
	private string _lastVehicleSortingOption = "registrationnumber";
	private bool _vehicleSortingDescending = false;
	private readonly Dictionary<string, Func<IVehicle, object>> _vehicleSortMethodsDict = new Dictionary<string, Func<IVehicle, object>>
		{
			{ "registrationnumber", vehicle => vehicle.RegistrationNumber },
			{ "make", vehicle => vehicle.Make },
			{ "vehicletype", vehicle => vehicle.VehicleType },
			{ "odometerposition", vehicle => vehicle.OdometerPosition },
			{ "costperkilometer", vehicle => vehicle.CostPerKilometer },
			{ "dailyrate", vehicle => vehicle.DailyRate },
		};
    private readonly BookingProcessor _bp;
    private readonly IData _dataAccess;

    public VehicleSorter(BookingProcessor bp, IData dataAccess)
    {
        _bp = bp;
        _dataAccess = dataAccess;
        _bp.DataChanged += () =>
        {
            SortVehicles(_lastVehicleSortingOption, _vehicleSortingDescending);
        };
    }

    public void SortVehicles(string propertyName, bool reorder = true)
	{
		propertyName = propertyName.ToLower();

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

		if (_vehicleSortMethodsDict.TryGetValue(propertyName, out var orderByExpression))
		{
			SortedVehicles = _vehicleSortingDescending ?
                _bp.Vehicles.OrderByDescending(orderByExpression).ToList() :
                _bp.Vehicles.OrderBy(orderByExpression).ToList();
		}
		else
		{
			throw new ArgumentException("Invalid sorting property");
		}
	}
}
