using Car_Rental.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Interfaces;

public interface IVehicle : IDataObject
{
    string RegistrationNumber { get; set; }
    string Make { get; set; }
    double OdometerPosition { get; set; }
    double CostPerKilometer { get; set; }
    VehicleType VehicleType { get; set; }
    double DailyRate { get; set; }
    BookingStatuses BookingStatus { get; set; }
}
