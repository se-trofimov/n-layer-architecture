namespace CatalogService.Application.Common;

public interface ISupportPagination
{
    int PageNumber { get; }
    int PageSize { get; }
}