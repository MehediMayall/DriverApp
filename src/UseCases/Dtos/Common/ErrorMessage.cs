using System.Text.Json.Serialization;
using Serilog;

namespace UseCases.Dtos;

public class ErrorMessage
{
    private ErrorMessage(string detail, string code, string title,  string url)
    {
        Details = detail;
        Title = title;
        Code = code;
        URL = url;
    }

    [JsonConstructor]
    public ErrorMessage()
    {

    }

    public string Code  { get; private set; }
    public string Title { get;  private set;}
    public string Details { get;  private set;}
    public string URL { get;   private set;}

    public static ErrorMessage Create(string Details, string Code ="", string Title = "",  string URL = "", object ReferenceObject = null)
    {
        if(string.IsNullOrEmpty(Details)) return null;
        var errorMessage = new ErrorMessage(Details, Code, Title, URL);
        
        try
        {
            Log.Error(Details);
            var referenceObjectInJSONString =  ReferenceObject.GetJsonString<object>();
            // Log.Information($"ReferenceObject: {referenceObjectInJSONString}");
        }
        catch(Exception ex){Log.Fatal(ex.GetAllExceptions());}

        return errorMessage;
    }
}

// public record ErrorMessage(string Code, string Title, string Details, string URL);

