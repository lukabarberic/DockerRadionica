using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TestLibrary.Models;

namespace TestLibrary
{
    public class UserService
    {
        private readonly IMongoCollection<User> _userCollection;
        public UserService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<User>(
                databaseSettings.Value.BooksCollectionName);
        }

        public async Task<List<User>> GetAsync()
        {
            var users = await _userCollection.Find(_ => true).ToListAsync();
            return users;
        }

        public async Task<User?> GetAsync(string id) =>
        await _userCollection.Find(x => x.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
        await _userCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, User updatedUser) =>
            await _userCollection.ReplaceOneAsync(x => x.Id == ObjectId.Parse(id), updatedUser);

        public async Task RemoveAsync(string id) =>
            await _userCollection.DeleteOneAsync(x => x.Id == ObjectId.Parse(id));


    }
}