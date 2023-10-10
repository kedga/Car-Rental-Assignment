using Car_Rental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Business.Classes;

public class VehicleSorter : BaseService
{
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

    public VehicleSorter(StateManagement stateManagement) : base(stateManagement)
    {
        stateManagement.DataChanged += () =>
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
			_stateManagement.Vehicles = _vehicleSortingDescending ?
                _stateManagement.Vehicles.OrderByDescending(orderByExpression).ToList() :
                _stateManagement.Vehicles.OrderBy(orderByExpression).ToList();
		}
		else
		{
			throw new ArgumentException("Invalid sorting property");
		}
	}
}
