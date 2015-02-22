using System;
using MongoDB.Bson;
using Xunit;

namespace MongoDBImplementationSample.Tests
{
    public class MyCounterServiceTests : TestBase
    {
        [Fact]
        public void HasEnoughRating_Should_Throw_InvalidOperationException_When_The_User_Is_Not_Found()
        {
            using (var server = StartServer())
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

        [Fact]
        public void HasEnoughRating_Should_Return_True_When_The_User_Has_More_Then_100_Rating()
        {
            using (var server = StartServer())
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

        [Fact]
        public void HasEnoughRating_Should_Return_False_When_The_User_Has_Less_Then_100_Rating()
        {
        }

        [Fact]
        public void HasEnoughRating_Should_Return_False_When_The_User_Has_100_Rating()
        {
        }
    }
}