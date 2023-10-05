using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;

namespace Car_Rental.Data.Classes;

public class DataCollection : IData
{
    private Dictionary<Type, List<object>> _dataObjects = new();
    private readonly IFetchData _fetchData;

    public DataCollection(IFetchData fetchData)
    {
        _fetchData = fetchData ?? throw new ArgumentNullException(nameof(fetchData));
    }
    public async Task FetchAndAddAsync<T>(string path, string filename) where T : IDataObject
    {
        IEnumerable<T>? data = await _fetchData.FetchDataAsync<T>(path, filename);

        if (data != null)
        {
            foreach (var item in data)
            {
                if (item != null)
                {
                    AddDataObject(item);
                }
            }
        }
    }

    public IEnumerable<T> GetDataObjectsOfType<T>() =>_dataObjects.TryGetValue(typeof(T), out var data) ? data!.Cast<T>() : Enumerable.Empty<T>();

    public void AddDataObject(IDataObject dataObject)
    {
        if (dataObject == null) throw new ArgumentNullException(nameof(dataObject));

        Type objectType = dataObject.GetType();

        foreach (var interfaceType in objectType.GetInterfaces())
        {
            if (!_dataObjects.ContainsKey(interfaceType)) _dataObjects[interfaceType] = new List<object>();

            _dataObjects[interfaceType].Add(dataObject);
        }

        if (!_dataObjects.ContainsKey(objectType)) _dataObjects[objectType] = new List<object>();

        _dataObjects[objectType].Add(dataObject);
    }

    public void PrintDataObjects()
    {
        Console.WriteLine("Contents of Data Objects:");

        foreach (var kvp in _dataObjects)
        {
            Type key = kvp.Key;
            IEnumerable<object> values = kvp.Value ?? Enumerable.Empty<object>();

            Console.WriteLine($"Type: {key.FullName}");
            Console.WriteLine("Values:");

            foreach (var value in values)
            {
                Console.WriteLine($"  {value}");
            }

            Console.WriteLine();
        }
    }

}
