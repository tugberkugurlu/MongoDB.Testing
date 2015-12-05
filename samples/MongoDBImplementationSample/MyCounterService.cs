using System;
using MongoDB.Driver;

namespace MongoDBImplementationSample
{
    public class MyCounterService
    {
        private readonly IMongoCollection<UserEntity> _userCollection;
        private const int Limit = 100;

        public MyCounterService(IMongoCollection<UserEntity> userCollection)
        {
            if (userCollection == null)
            {
                throw new ArgumentNullException(nameof(userCollection));
            }

            _userCollection = userCollection;
        }

        public bool HasEnoughRating(string userId)
        {
            UserEntity userEntity = _userCollection.Find(Builders<UserEntity>.Filter.Eq(user => user.Id, userId)).FirstOrDefaultAsync().GetAwaiter().GetResult();
            if (userEntity == null)
            {
                throw new InvalidOperationException("Cannot see the rating of an unexisting user.");
            }

            return userEntity.Rating > Limit;
        }
    }
}
