namespace CartingService.Configuration
{
    public class CosmosDbSettings
    {
        public string CosmosDbConnectionString { get; init; }
        public string CosmosDbName { get; init; }
        public string CosmosDbContainerName { get; init; }
        public string CosmosDbContainerPartitionKey { get; init; }
        public int CosmosDbThroughput { get; init; }
    }
}

