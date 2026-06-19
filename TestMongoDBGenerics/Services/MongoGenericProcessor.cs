using System.Dynamic;
using TestMongoDBGenerics.Repositories;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace TestMongoDBGenerics.Services;

public class MongoGenericProcessor(IMongoGenericRepository mongoGenericRepository) : IMongoGenericProcessor
{
    public async Task<object> ExecuteAsync(string body)
    {
        dynamic obj = JsonConvert.DeserializeObject<ExpandoObject>(body, 
            new Newtonsoft.Json.Converters.ExpandoObjectConverter())!;

        string action = obj.action;

        return action switch
        {
            "create" => await mongoGenericRepository.CreateRecord(obj.collection, obj.data),
            "delete" => await mongoGenericRepository.DeleteRecord(obj.collection, obj.data),
            "get" => await mongoGenericRepository.GetCollectionById(obj.collection, obj.data),
            "list" => await mongoGenericRepository.GetCollectionList(obj.collection),
            "update" => await mongoGenericRepository.UpdateRecord(obj.collection, obj.data),
            _ => throw new ArgumentException("Invalid action"),
        };
    }
}
