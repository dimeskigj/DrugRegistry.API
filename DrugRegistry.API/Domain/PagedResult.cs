namespace DrugRegistry.API.Domain;

public class PagedResult<T>
{
    public IEnumerable<T> Data { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
    
    public PagedResult(IEnumerable<T> data, int total, int page, int size)
    {
        Data = data;
        TotalCount = total;
        Page = page;
        Size = size;
    }
}