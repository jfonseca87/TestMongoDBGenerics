using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TestMongoDBGenerics.Repositories;
using TestMongoDBGenerics.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMongoClient>(
    new MongoClient("mongodb://localhost:27017")
);

builder.Services.AddTransient<IMongoGenericRepository, MongoGenericRepository>();
builder.Services.AddTransient<IMongoGenericProcessor, MongoGenericProcessor>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/api/genericaction", async (
    HttpContext httpContext, 
    [FromServices] IMongoGenericProcessor mongoGenericProcessor
) =>
{
    using var reader = new StreamReader(httpContext.Request.Body);
    var body = await reader.ReadToEndAsync();

    object result = await mongoGenericProcessor.ExecuteAsync(body);

    return Results.Ok(result);
})
.WithName("AddDataToCollection")
.WithOpenApi();

app.Run();