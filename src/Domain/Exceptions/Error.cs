using Domain.Shared;
using Serilog;


namespace Domain.Exceptions;

public sealed record Error<T>where T : class
{
    public Error(string Message, string Code = null) 
    {
        try
        {
            // if(!string.IsNullOrEmpty(Message)) Log.Error(Message, Code);
        }
        catch (Exception ex){Log.Fatal(ex.GetAllExceptions());}

        this.Message = Message;
        this.Code = Code;
    }
    public static readonly Error<T> None = new(string.Empty);

    public string Message { get; }
    public string Code { get; }

    public static Error<T> Set(string Message, string Code="") => new(Message, Code);

    public static implicit operator Result<T> (Error<T> error) => Result<T>.Failure(error);

}