using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace MongoDBImplementationSample
{
    public class MyCounterService
    {
        private readonly MongoCollection<UserEntity> _userCollection;
        private const int Limit = 100;

        public MyCounterService(MongoCollection<UserEntity> userCollection)
        {
            if (userCollection == null)
            {
                throw new ArgumentNullException("userCollection");
            }

            _userCollection = userCollection;
        }

        public bool HasEnoughRating(string userId)
        {
            UserEntity userEntity = _userCollection.FindOne(Query<UserEntity>.EQ(user => user.Id, userId));
            if (userEntity == null)
            {
                throw new InvalidOperationException("Cannot see the rating of an unexisting user.");
            }

            return userEntity.Rating > Limit;
        }
    }
}
