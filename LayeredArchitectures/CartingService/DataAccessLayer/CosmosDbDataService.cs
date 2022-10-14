using System.Net;
using Microsoft.Azure.Cosmos;

namespace CartingService.DataAccessLayer
{
    public class CosmosDbDataService
    {

        private readonly CosmosClient _client;
        

        public CosmosDbDataService(CosmosClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        internal async Task<Database> GetOrCreateDbAsync(string dbName)
            => await _client.CreateDatabaseIfNotExistsAsync(id: dbName);

        internal async Task<Container> GetOrCreateContainerAsync(string dbName, string containerName, 
            string partitionKey, int throughput = 400)
        {
            var dbResult = await _client.CreateDatabaseIfNotExistsAsync(id: dbName);
            if (dbResult.StatusCode is not (HttpStatusCode.OK or HttpStatusCode.Created))
                throw new Exception("Unable to connect CosmosDb");

            var containerResult = await dbResult.Database.CreateContainerIfNotExistsAsync(
                id: containerName,
                partitionKeyPath: partitionKey,
                throughput: throughput);
            
            if (containerResult.StatusCode is HttpStatusCode.OK or HttpStatusCode.Created)
                return containerResult.Container;
            
            throw new Exception("Unable to connect CosmosDb");
        }
    }
}
