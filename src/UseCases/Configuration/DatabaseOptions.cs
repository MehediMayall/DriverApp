namespace UseCases.Dtos;

public class DatabaseOptions
{
    public string ConnectionString {get; set;} = string.Empty;
    public int MaxRetryCount {get; set;}
    public int CommandTimeout {get; set;}
    public bool EnableDetailedErrors {get; set;}
    public bool EnableSensitiveDataLoggin {get; set;}
}