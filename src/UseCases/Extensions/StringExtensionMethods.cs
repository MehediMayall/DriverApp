namespace UseCases.Extensions;

public static class StringExtensionMethods
{
    public static string? ToUpper(this object Object) => Object is null ? null : Object.ToString()!.Trim().ToUpper();
    public static bool IsEmptyOrWhiteSpace(this string? value) => string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

    public static T? ConvertTo<T>(this string @JsonString) => @JsonString is null ? default : JsonSerializer.Deserialize<T>(@JsonString);

}
