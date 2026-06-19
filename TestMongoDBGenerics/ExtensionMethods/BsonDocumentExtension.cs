using MongoDB.Bson;
using System.Dynamic;

namespace TestMongoDBGenerics.ExtensionMethods;

public static class BsonDocumentExtension
{
    public static ExpandoObject ToExpando(this BsonDocument doc)
    {
        IDictionary<string, object> expando = new ExpandoObject()!;
        foreach (var element in doc)
        {
            string name = element.Name.Equals("_id") ? "id" : element.Name;
            expando[name] = BsonValueToObject(element.Value);
        }
        return (ExpandoObject)expando!;
    }

    public static List<ExpandoObject> ToExpandoList(this List<BsonDocument> docs)
    {
        List<ExpandoObject> list = new();
        foreach (var doc in docs)
        {
            list.Add(doc.ToExpando());
        }
        return list;
    }

    private static object BsonValueToObject(BsonValue value) =>
        value?.BsonType switch
        {
            BsonType.Document => ToExpando(value.AsBsonDocument),
            BsonType.Array => value.AsBsonArray.Select(BsonValueToObject).ToList(),
            BsonType.ObjectId => value.AsObjectId.ToString(),
            BsonType.Boolean => value.AsBoolean,
            BsonType.DateTime => value.ToUniversalTime(),
            BsonType.Double => value.AsDouble,
            BsonType.Int32 => value.AsInt32,
            BsonType.Int64 => value.AsInt64,
            BsonType.String => value.AsString,
            _ => value?.ToString() ?? string.Empty
        };
}
