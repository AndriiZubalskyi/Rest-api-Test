using RestAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace RestAPI.Services
{
    public class OperationService
    {
        private readonly IMongoCollection<OperationModel> _operationsCollection;

        public OperationService(
            IOptions<DataDatabaseSettings> dataDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                dataDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                dataDatabaseSettings.Value.DatabaseName);

            _operationsCollection = mongoDatabase.GetCollection<OperationModel>(
                dataDatabaseSettings.Value.OperationsCollectionName);
        }

        public async Task<List<OperationModel>> GetAsync() =>
            await _operationsCollection.Find(_ => true).ToListAsync();

        public async Task<OperationModel?> GetAsync(string id) =>
            await _operationsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(OperationModel newOperation) =>
            await _operationsCollection.InsertOneAsync(newOperation);

        public async Task UpdateAsync(string id, OperationModel updatedOperation) =>
            await _operationsCollection.ReplaceOneAsync(x => x.Id == id, updatedOperation);

        public async Task RemoveAsync(string id) =>
            await _operationsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
