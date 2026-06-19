# TestMongoDBGenerics

A .NET 8 Minimal API that provides a **generic CRUD endpoint for MongoDB**. The API accepts a JSON payload containing an `action`, `collection`, and `data`, then dynamically routes to the appropriate MongoDB operation — all without requiring predefined models or schemas.

The core idea is to treat any MongoDB collection generically: the client specifies the collection name at runtime, and the system uses `BsonDocument` and `ExpandoObject` for dynamic document handling. This is useful for rapid prototyping or admin tools where collections and schemas are not known at compile time.

## Technologies / Libraries

- **.NET 8** (ASP.NET Core Minimal API)
- **MongoDB.Driver** 3.1.0 — Official MongoDB driver
- **Newtonsoft.Json** 13.0.3 — JSON deserialization with `ExpandoObjectConverter`
- **Swashbuckle.AspNetCore** 6.6.2 — Swagger UI

## Supported Actions

| Action | Description | Route |
|---|---|---|
| `create` | Insert a new document | `POST /api/genericaction` |
| `get` | Retrieve a document by `_id` | `POST /api/genericaction` |
| `list` | List all documents in a collection | `POST /api/genericaction` |
| `update` | Update a document by `_id` | `POST /api/genericaction` |
| `delete` | Delete a document by `_id` | `POST /api/genericaction` |

All actions are sent as a JSON body to the single endpoint:
```json
{
  "action": "create",
  "collection": "books",
  "data": { "title": "Dune", "author": "Frank Herbert" }
}
```

## Key Features

- **Generic repository** — `MongoGenericRepository` operates on `IMongoCollection<BsonDocument>` with dynamic `ExpandoObject` input.
- **BSON-to-Expando conversion** — `BsonDocumentExtension` maps BSON types to .NET types recursively (ObjectId → string, nested documents → nested ExpandoObjects, etc.).
- **Single endpoint** — `POST /api/genericaction` handles all CRUD operations via a `switch` expression.

## How to Run

1. Ensure MongoDB is running on `localhost:27017`.
2. Run the project:
   ```bash
   dotnet run --project TestMongoDBGenerics
   ```
3. Access Swagger UI at `http://localhost:5000/swagger`.
4. Send a JSON body to `POST /api/genericaction` with action, collection, and data fields.
