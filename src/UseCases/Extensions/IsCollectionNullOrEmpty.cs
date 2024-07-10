namespace UseCases.Extensions;

public static class IsCollectionNullOrEmpty
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        if(enumerable != null) return !enumerable.Any();
        return true;
    }
}