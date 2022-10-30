namespace CatalogService.Application.Common;

public abstract class SupportPagination : ISupportPagination
{
    protected SupportPagination(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public int PageNumber { get; }
    public int PageSize { get; }
}