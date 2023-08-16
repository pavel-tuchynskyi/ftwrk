using MongoDB.Driver;

namespace FTWRK.Persistance.Common.Interfaces
{
    public interface IMongoContext
    {
        IMongoCollection<TData> GetCollection<TData>();
        string GetCollectionName<TData>();
        Task<IClientSessionHandle> StartSessionAsync();
    }
}
