using Car_Rental.Common.Classes;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System.Linq;

namespace Car_Rental.Data.Classes;

public class FakeDatabase2 : IData
{
	private readonly IFetchData _fetchData;
	public FakeDatabase2(IFetchData fetchData)
	{
		_fetchData = fetchData;
	}
	private readonly List<object> _data = new();

    public List<T> Get<T>(Func<T, bool> filter) =>
    _data.OfType<T>().Where(filter).ToList();
    public T? Single<T>(Func<T, bool> filter)
    {
        return _data.OfType<T>().Where(filter).FirstOrDefault();
    }

    public Task Add(IDataObject dataObject)
	{
		if (dataObject is null) throw new InvalidOperationException("dataObject is null in AddDataObject.");

		_data.Add(dataObject);

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
			Get<IBooking>(b => true),
			Get<IVehicle>(v => true),
			outerEntity => outerEntity.RegistrationNumber,
			innerEntity => innerEntity.RegistrationNumber,
			(booking, vehicle) => booking.Vehicle = vehicle);
		DataMatching.MatchAndInsert(
			Get<IBooking>(b => true),
			Get<IPerson>(p => true),
			outerEntity => outerEntity.CustomerSsn,
			innerEntity => innerEntity.SocialSecurityNumber,
			(booking, customer) => booking.Customer = customer);
	}
}
