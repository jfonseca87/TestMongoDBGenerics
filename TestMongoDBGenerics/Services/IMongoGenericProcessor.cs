namespace TestMongoDBGenerics.Services;

public interface IMongoGenericProcessor
{
    Task<object> ExecuteAsync(string body);
}
