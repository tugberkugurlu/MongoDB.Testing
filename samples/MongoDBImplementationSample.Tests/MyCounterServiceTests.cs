using System;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Testing.Mongo;
using NUnit.Framework;

namespace MongoDBImplementationSample.Tests
{
    public class MyCounterServiceTests : TestBase
    {
        [Test]
        public void HasEnoughRating_Should_Throw_InvalidOperationException_When_The_User_Is_Not_Found()
        {
            using (MongoTestServer server = StartServer())
            {
                // ARRANGE
                var collection = server.Database.GetCollection<UserEntity>("users");
                var service = new MyCounterService(collection);
                collection.Insert(new UserEntity
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

        [Test]
        public void HasEnoughRating_Should_Return_True_When_The_User_Has_More_Then_100_Rating()
        {
            using (MongoTestServer server = StartServer())
            {
                // ARRANGE
                var collection = server.Database.GetCollection<UserEntity>("users");
                var service = new MyCounterService(collection);
                var userId = ObjectId.GenerateNewId().ToString();
                collection.Insert(new UserEntity
                {
                    Id = userId,
                    Name = "foo",
                    Rating = 101
                });

                // ACT
                bool isEnough = service.HasEnoughRating(userId);

                // ASSERT
                Assert.True(isEnough);
            }
        }

        [Test]
        public void HasEnoughRating_Should_Return_False_When_The_User_Has_Less_Then_100_Rating()
        {
            using (MongoTestServer server = StartServer())
            {
                // ARRANGE
                var collection = server.Database.GetCollection<UserEntity>("users");
                var service = new MyCounterService(collection);
                var userId = ObjectId.GenerateNewId().ToString();
                collection.Insert(new UserEntity
                {
                    Id = userId,
                    Name = "foo",
                    Rating = 90
                });

                // ACT
                bool isEnough = service.HasEnoughRating(userId);

                // ASSERT
                Assert.False(isEnough);
            }
        }

        [Test]
        public void HasEnoughRating_Should_Return_False_When_The_User_Has_100_Rating()
        {
            using (MongoTestServer server = StartServer())
            {
                // ARRANGE
                var collection = server.Database.GetCollection<UserEntity>("users");
                var service = new MyCounterService(collection);
                var userId = ObjectId.GenerateNewId().ToString();
                collection.Insert(new UserEntity
                {
                    Id = userId,
                    Name = "foo",
                    Rating = 100
                });

                // ACT
                bool isEnough = service.HasEnoughRating(userId);

                // ASSERT
                Assert.False(isEnough);
            }
        }
    }
}