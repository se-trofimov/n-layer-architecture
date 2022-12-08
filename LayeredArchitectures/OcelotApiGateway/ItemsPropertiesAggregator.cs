using System.Buffers;
using System.Net;
using System.Text.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using OcelotApiGateway.AggregatedModels;

namespace OcelotApiGateway;
public sealed class ItemsPropertiesAggregator : IDefinedAggregator
{
    public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
    {
        foreach (var response in responses)
        {
            var dsResp = response.Items.DownstreamResponse();
            if (dsResp.StatusCode != HttpStatusCode.OK)
                return new DownstreamResponse(dsResp.Content, dsResp.StatusCode, dsResp.Headers, "");
        }

        var propertiesResponse = responses.First(x => x.Items.DownstreamRoute()
            .DownstreamPathTemplate.Value.EndsWith("properties"));
        var itemsResponse = responses.First(x => !x.Items.DownstreamRoute()
            .DownstreamPathTemplate.Value.EndsWith("properties"));


        var propertiesStream = await propertiesResponse.Items.DownstreamResponse().Content.ReadAsStringAsync();
        var itemsStream = await itemsResponse.Items.DownstreamResponse().Content.ReadAsStringAsync();

        var jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        var item = JsonSerializer.Deserialize<ItemDto>(itemsStream, jsonOptions);
        var properties = JsonSerializer.Deserialize<Dictionary<string, string>>(propertiesStream, jsonOptions);
        
        item.Properties = properties;

        HttpContent content = new StringContent(JsonSerializer.Serialize(item));
        return new DownstreamResponse(content, HttpStatusCode.OK, responses[0].Items.DownstreamResponse().Headers, "");
    }
}
