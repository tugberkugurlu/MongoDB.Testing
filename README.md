## MongoDB.Testing

MongoDB integration testing helper from .NET projects that stands up a server and a random database on the fly. 
The library is compatible with .NET 4.5.1 and [.NET Standard](https://docs.microsoft.com/en-us/dotnet/articles/standard/library) 1.6.

## Quick Start

Install the library into your testing project through NuGet:

```
Install-Package MongoDB.Testing -pre
```

Write a `mongod.exe` locator:

```csharp
public class MongodExeLocator : IMongoExeLocator
{
    public string Locate()
    {
        return @"C:\Program Files\MongoDB\Server\3.0\bin\mongod.exe";
    }
}
```

Integrate it into your tests:

```csharp
[Test]
public async Task HasEnoughRating_Should_Throw_InvalidOperationException_When_The_User_Is_Not_Found()
{
    using (MongoTestServer server = MongoTestServer.Start(27017, new MongodExeLocator()))
    {
        // ARRANGE
        var collection = server.Database.GetCollection<UserEntity>("users");
        var service = new MyCounterService(collection);
        await collection.InsertOneAsync(new UserEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "foo",
            Rating = 23
        });

        // ACT, ASSERT
        Assert.Throws<InvalidOperationException>(
            () => service.HasEnoughRating(ObjectId.GenerateNewId().ToString()));
    }
}
```

`MongoTestServer.Start` will do the following:

 - Start a `mongod` instance and expose it through the specified port.
 - Creates a randomly named MongoDB database on the started instance and exposes it through the `MongoTestServer` instance returned from `MongoTestServer.Start` method.
 - Cleans up the resources, kills the mongod.exe instance when the `MongoTestServer` instance is disposed.