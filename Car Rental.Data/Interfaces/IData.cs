namespace Car_Rental.Data.Interfaces
{
    public interface IData
    {
        void Add<T>(T item) where T : class;
        List<T> Get<T>(Func<T, bool>? filter) where T : class;
        T? Single<T>(Func<T, bool>? filter = null) where T : class;
        Task Initialize();
        void Remove<T>(Func<T, bool> predicate) where T : class;
    }
}