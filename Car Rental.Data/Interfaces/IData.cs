using Car_Rental.Common.Interfaces;

namespace Car_Rental.Data.Interfaces;

public interface IData
{
    Task<IEnumerable<T>> GetDataObjectsOfType<T>();
    Task AddDataObject(IDataObject dataObject);
    void PrintDataObjects();
    Task FetchAndAddAsync<T>(string path, string filename) where T : IDataObject;
}