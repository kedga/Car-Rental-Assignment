﻿using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Common.Utilities;

namespace Car_Rental.Business.Classes;

public class AddVehicle
{
    private readonly BookingProcessor _bp;
    public AddVehicle(BookingProcessor bp)
    {
        _bp = bp;
    }
    private string _odometerPositionInput = string.Empty;
    public string OdometerPositionInput
    {
        get { return _odometerPositionInput; }
        set { _odometerPositionInput = value.FormatAsInt(); }
    }

    private string _costPerKilometerInput = string.Empty;
    public string CostPerKilometerInput
    {
        get { return _costPerKilometerInput; }
        set { _costPerKilometerInput = value.FormatAsDouble(2); }
    }

    private string _dailyRateInput = string.Empty;
    public string DailyRateInput
    {
        get { return _dailyRateInput; }
        set { _dailyRateInput = value.FormatAsDouble(2); }
    }
    public IVehicle PrototypeVehicle { get; set; } = new Vehicle("", "");
    public void SelectVehicleType(VehicleType vehicleType)
    {
        PrototypeVehicle.VehicleType = vehicleType;
    }

    public bool DataIsValid()
    {
        if (string.IsNullOrWhiteSpace(PrototypeVehicle.RegistrationNumber) ||
            string.IsNullOrWhiteSpace(PrototypeVehicle.Make) ||
            !OdometerPositionInput.IsPositive() ||
            !CostPerKilometerInput.IsPositive() ||
            !DailyRateInput.IsPositive())
        {
            return false;
        }
        return true;
    }
    public bool IsRegNoUnique()
    {
        if (PrototypeVehicle.RegistrationNumber.StringCollisionCheck(_bp.Vehicles.Select(vehicle => vehicle.RegistrationNumber), "[A-ZÅÄÖa-zåäö0-9]", true))
        {
            return false;
        }
        return true;
    }
    public void EnterCommonVehicleData(int inputGroup)
    {
        _bp.ValidateAggressivelyDict[inputGroup] = true;

        if (IsRegNoUnique() is not true)
        {
            PrototypeVehicle.RegistrationNumber = string.Empty;
            _bp.SetErrorMessage("Registration number already exists.");
            return;
        }

        if (DataIsValid() is not true)
        {
            _bp.SetErrorMessage("Please fill in all required fields and ensure numeric values are positive.");
            return;
        }

        if (PrototypeVehicle.VehicleType is VehicleType.None)
        {
            _bp.SetErrorMessage("Please select a vehicle type.");
            return;
        }

        switch (PrototypeVehicle.VehicleType)
        {
            case VehicleType.Sedan:
                PrototypeVehicle = new Sedan(PrototypeVehicle.RegistrationNumber, PrototypeVehicle.Make);
                PrototypeVehicle.VehicleType = VehicleType.Sedan;
                break;
            case VehicleType.Van:
                PrototypeVehicle = new Van(PrototypeVehicle.RegistrationNumber, PrototypeVehicle.Make);
                PrototypeVehicle.VehicleType = VehicleType.Van;
                break;
            case VehicleType.StationWagon:
                PrototypeVehicle = new StationWagon(PrototypeVehicle.RegistrationNumber, PrototypeVehicle.Make);
                PrototypeVehicle.VehicleType = VehicleType.StationWagon;
                break;
            case VehicleType.Motorcycle:
                PrototypeVehicle = new Motorcycle(PrototypeVehicle.RegistrationNumber, PrototypeVehicle.Make);
                PrototypeVehicle.VehicleType = VehicleType.Motorcycle;
                break;
        }

        PrototypeVehicle.CostPerKilometer = decimal.TryParse(CostPerKilometerInput, out decimal cpkOut) ? cpkOut : 0.0m;
        PrototypeVehicle.DailyRate = decimal.TryParse(DailyRateInput, out decimal dailyRateOut) ? dailyRateOut : 0.0m;
        PrototypeVehicle.OdometerPosition = double.TryParse(OdometerPositionInput, out double odometerOut) ? odometerOut : 0;
        _bp.ValidateAggressivelyDict[inputGroup] = false;
    }
    public void EnterSpecificVehicleData(int inputGroup)
    {
        _bp.AddDataObject(PrototypeVehicle);

        _bp.RefreshData();

        PrototypeVehicle = new Vehicle("", "");
        OdometerPositionInput = CostPerKilometerInput = DailyRateInput = string.Empty;
        _bp.ValidateAggressivelyDict[inputGroup] = false;
        _bp.ClearErrorMessage();
    }
}