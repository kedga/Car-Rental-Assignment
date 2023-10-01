using Car_Rental.Common.Interfaces;

namespace Car_Rental.Data.Interfaces;


public interface IFetchData
{
    Task<List<T>> FetchDataAsync<T>(string path, string filename);
}