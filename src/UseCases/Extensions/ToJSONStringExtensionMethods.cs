namespace UseCases.Extensions;

public static class ToJsonStringExtensionMethods
{
    public static string GetJsonString<T>(this T DataObject) => DataObject is null ? default : JsonSerializer.Serialize<T>(DataObject);
}