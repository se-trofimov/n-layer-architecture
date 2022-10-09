using CartingService.DataAccessLayer.Entities;
using Microsoft.Azure.Cosmos;

namespace CartingService.DataAccessLayer
{
    public class CartRepository : ICartRepository
    {
        private readonly CosmosDbDataService _cosmosDbDataService;
        private readonly string _dbName;
        private readonly string _containerName;
        private readonly string _partitionKey;
        private readonly int _throughput;

        public CartRepository(CosmosDbDataService cosmosDbDataService,
            string dbName,
            string containerName,
            string partitionKey,
            int throughput = 400)

        {
            _cosmosDbDataService = cosmosDbDataService ?? throw new ArgumentNullException(nameof(cosmosDbDataService));
            _dbName = dbName ?? throw new ArgumentNullException(nameof(dbName));
            _containerName = containerName ?? throw new ArgumentNullException(nameof(containerName));
            _partitionKey = partitionKey ?? throw new ArgumentNullException(nameof(partitionKey));
            _throughput = throughput;
        }

        public async Task<Cart> AddAsync(Cart cart)
        {
            var container = await _cosmosDbDataService.GetOrCreateContainerAsync(_dbName, _containerName, _partitionKey, _throughput);
            return await container.UpsertItemAsync(cart, new PartitionKey(cart.Id));
        }

        public async Task RemoveAsync(Cart cart)
        {
            var container = await _cosmosDbDataService.GetOrCreateContainerAsync(_dbName, _containerName, _partitionKey, _throughput);
            await container.DeleteItemAsync<Cart>(cart.Id, new PartitionKey(_partitionKey));
        }

        public async Task<Cart> GetItemById(int id)
        {
            var container = await _cosmosDbDataService.GetOrCreateContainerAsync(_dbName, _containerName, _partitionKey, _throughput);
            return await
                container.ReadItemAsync<Cart>(id.ToString(), new PartitionKey(_partitionKey));
        }
    }
}
