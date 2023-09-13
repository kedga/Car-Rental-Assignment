using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Data.Interfaces;

public interface IData
{
    Task <IEnumerable<IBooking>> GetBookingsAsync();
    Task <IEnumerable<IVehicle>> GetVehiclesAsync();
    Task<IEnumerable<IPerson>> GetPeopleAsync();
    string GetErrorMessage();
}