namespace DrugRegistry.API.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> FilterNull<T>(this IEnumerable<T?> enumerable)
    {
        return enumerable.Where(e => e is not null)!;
    }
}