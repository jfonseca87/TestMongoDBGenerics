using MongoDB.Bson;
using MongoDB.Driver;
using System.Dynamic;
using TestMongoDBGenerics.ExtensionMethods;

namespace TestMongoDBGenerics.Repositories;

public class MongoGenericRepository(IMongoClient mongoClient) : IMongoGenericRepository
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase("testDb");

    public async Task<string> CreateRecord(string collectionName, dynamic data)
    {
        var collection = _database.GetCollection<BsonDocument>(collectionName);

        var document = new BsonDocument();
        foreach (var item in data)
        {
            document.Add(item.Key, item.Value.ToString());
        }

        await collection.InsertOneAsync(document);

        document.TryGetElement("_id", out BsonElement idElement);
        return idElement.Value.ToString()!;
    }

    public async Task<bool> DeleteRecord(string collectionName, dynamic data)
    {
        string recordId = data.id;
        ObjectId.TryParse(recordId, out ObjectId objectId);

        var collection = _database.GetCollection<BsonDocument>(collectionName);

        var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
        DeleteResult result = await collection.DeleteOneAsync(filter);

        return result.IsAcknowledged;
    }

    public async Task<object> GetCollectionById(string collectionName, dynamic data)
    {
        string recordId = data.id;
        ObjectId.TryParse(recordId, out ObjectId objectId);

        var collection = _database.GetCollection<BsonDocument>(collectionName);

        var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
        var result = await collection.Find(filter).FirstOrDefaultAsync();

        return result.ToExpando();
    }

    public async Task<object> GetCollectionList(string collectionName)
    {
        var collection = _database.GetCollection<BsonDocument>(collectionName);
        // Create custom builder for filter a collection list
        var result = await collection.Find(FilterDefinition<BsonDocument>.Empty).ToListAsync();

        return result.ToExpandoList();
    }

    public async Task<bool> UpdateRecord(string collectionName, dynamic data)
    {
        string recordId = data.id;
        ObjectId.TryParse(recordId, out ObjectId objectId);

        var collection = _database.GetCollection<BsonDocument>(collectionName);

        var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
        var dataToUpdate = new BsonDocument();
        foreach (var item in data)
        {
            if (item.Key == "id")
                continue;

            dataToUpdate.Add(item.Key, item.Value.ToString());
        }
        var updateRecord = new BsonDocument("$set", dataToUpdate);

        var result = await collection.UpdateOneAsync(filter, updateRecord);

        return result.IsAcknowledged;
    }
}
