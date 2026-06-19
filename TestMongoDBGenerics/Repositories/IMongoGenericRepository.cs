namespace TestMongoDBGenerics.Repositories;

public interface IMongoGenericRepository
{
    Task<object> GetCollectionList(string collectionName);
    Task<object> GetCollectionById(string collectionName, dynamic data);
    Task<string> CreateRecord(string collectionName, dynamic data);
    Task<bool> UpdateRecord(string collectionName, dynamic data);
    Task<bool> DeleteRecord(string collectionName, dynamic data);
}
