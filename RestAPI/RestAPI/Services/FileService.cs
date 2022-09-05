using RestAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace RestAPI.Services
{
    public class FileService
    {
        private readonly IMongoCollection<FileModel> _filesCollection;

        public FileService(
            IOptions<DataDatabaseSettings> dataDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                dataDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                dataDatabaseSettings.Value.DatabaseName);

            _filesCollection = mongoDatabase.GetCollection<FileModel>(
                dataDatabaseSettings.Value.FilesCollectionName);
        }

        public async Task<List<FileModel>> GetAsync() =>
            await _filesCollection.Find(_ => true).ToListAsync();

        public async Task<FileModel?> GetAsync(string id) =>
            await _filesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(FileModel newFile) =>
            await _filesCollection.InsertOneAsync(newFile);

        public async Task UpdateAsync(string id, FileModel updatedFile) =>
            await _filesCollection.ReplaceOneAsync(x => x.Id == id, updatedFile);

        public async Task RemoveAsync(string id) =>
            await _filesCollection.DeleteOneAsync(x => x.Id == id);
    }
}
