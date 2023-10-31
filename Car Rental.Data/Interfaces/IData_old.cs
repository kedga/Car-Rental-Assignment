using Car_Rental.Common.Interfaces;

namespace Car_Rental.Data.Interfaces
{
    public interface IData_old
    {
        Task Add(IDataObject dataObject);
        List<T> Get<T>(Func<T, bool> filter);
        Task Initialize();
        T? Single<T>(Func<T, bool> filter);
    }
}