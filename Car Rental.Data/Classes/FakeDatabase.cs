using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;

namespace Car_Rental.Data.Classes;

public class FakeDatabase : IData
{
    private readonly IFetchData _fetchData;
    public FakeDatabase(IFetchData fetchData)
    {
        _fetchData = fetchData;
    }
    private readonly Dictionary<Type, List<object>> _data = new();
    Task<IEnumerable<T>> IData.GetDataObjectsOfType<T>()
    {
        return Task.FromResult(_data.TryGetValue(typeof(T), out var dataList) ? dataList!.Cast<T>() : Enumerable.Empty<T>());
    }

    public Task AddDataObject(IDataObject dataObject)
    {
        if (dataObject == null) throw new ArgumentNullException(nameof(dataObject));

        Type objectType = dataObject.GetType();

        foreach (var interfaceType in objectType.GetInterfaces())
        {
            if (!_data.ContainsKey(interfaceType)) _data[interfaceType] = new List<object>();

            _data[interfaceType].Add(dataObject);
        }

        if (!_data.ContainsKey(objectType)) _data[objectType] = new List<object>();

        _data[objectType].Add(dataObject);
        return Task.CompletedTask;
    }

    async Task IData.FetchAndAddAsync<T>(string path, string filename)
    {
        IEnumerable<T>? data = await _fetchData.FetchDataAsync<T>(path, filename);

        if (data != null)
        {
            foreach (var item in data)
            {
                if (item != null)
                {
                    await AddDataObject(item);
                }
            }
        }
    }
    void IData.PrintDataObjects()
    {
        throw new NotImplementedException();
    }
}
