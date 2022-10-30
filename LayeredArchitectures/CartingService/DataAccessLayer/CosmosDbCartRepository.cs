using System.Net;
using CartingService.DataAccessLayer.Entities;
using CartingService.Exceptions;
using Microsoft.Azure.Cosmos;

namespace CartingService.DataAccessLayer
{
    public class CosmosDbCartRepository : ICartRepository
    {
        private readonly CosmosDbDataService _cosmosDbDataService;
        private readonly string _dbName;
        private readonly string _containerName;
        private readonly string _partitionKey;
        private readonly int _throughput;

        public CosmosDbCartRepository(CosmosDbDataService cosmosDbDataService,
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
            return await container.UpsertItemAsync(cart, new PartitionKey(cart.Id.ToString()));
        }

        public async Task<Cart> UpdateAsync(Cart cart)
        {
            var container = await _cosmosDbDataService.GetOrCreateContainerAsync(_dbName, _containerName, _partitionKey, _throughput);
            return await container.UpsertItemAsync(cart, new PartitionKey(cart.Id.ToString()));
        }

        public async Task RemoveAsync(Cart cart)
        {
            var container = await _cosmosDbDataService.GetOrCreateContainerAsync(_dbName, _containerName, _partitionKey, _throughput);
            await container.DeleteItemAsync<Cart>(cart.Id.ToString(), new PartitionKey(_partitionKey));
        }

        public async Task<Cart> GetItemById(Guid id)
        {
            var container = await _cosmosDbDataService.GetOrCreateContainerAsync(_dbName, _containerName, _partitionKey, _throughput);
            try
            {
                var response = await
                    container.ReadItemAsync<Cart>(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource;
            }
            catch (CosmosException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return await UpdateAsync(new Cart() { Id = id });
                }
                else throw;
            }
        }
    }
}
