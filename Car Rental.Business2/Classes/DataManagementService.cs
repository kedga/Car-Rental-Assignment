using Car_Rental.Common.Classes;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Business.Classes
{
    public class DataManagementService
    {
        private readonly IData _dataAccess;
        public DataManagementService(IData dataAccess) => _dataAccess = dataAccess;
        public IData DataAccess => _dataAccess;
        public Dictionary<int, bool> ValidateAggressivelyDict { get; set; } = Enumerable.Range(1, 10).ToDictionary(x => x, x => false);
        public string errorMessage = string.Empty;
        public List<IVehicle> Vehicles { get; set; } = new List<IVehicle>();
        public List<IPerson> Customers { get; set; } = new List<IPerson>();
        public List<IBooking> Bookings { get; set; } = new List<IBooking>();
        public IEnumerable<T> GetDataObjectsOfType<T>() where T : class => _dataAccess.GetDataObjectsOfType<T>();
        public void ClearErrorMessage() => errorMessage = string.Empty;
        public void AddDataObject(IDataObject dataObject) => _dataAccess.AddDataObject(dataObject);
        public async Task InitializeDataAsync()
        {
            await _dataAccess.FetchAndAddAsync<Vehicle>("data/", "vehicles.json");
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
        }
    }
}
