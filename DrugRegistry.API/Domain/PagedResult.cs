namespace DrugRegistry.API.Domain;

public class PagedResult<T>(IEnumerable<T> data, int total, int page, int size)
{
    public IEnumerable<T> Data { get; set; } = data;
    public int TotalCount { get; set; } = total;
    public int Page { get; set; } = page;
    public int Size { get; set; } = size;
}