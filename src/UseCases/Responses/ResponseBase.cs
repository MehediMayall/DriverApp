using System.Text.Json.Serialization;

namespace UseCases.Responses;

public class Response<T> where T : class 
{
    public string Status { get; private set; } 
    public T? Data { get; private set; }
    public List<ErrorMessage> Errors {get;set;} = default!;

    private Response()
    {
        Errors =  new List<ErrorMessage>();
        Status = ResponseStatus.ERROR;
    }

    [JsonConstructor]
    public Response(T? Data)
    {
        Status = ResponseStatus.OK;
        this.Data = Data;
    }

    private Response(T? Data, string test="")
    {
        Status = ResponseStatus.OK;
        this.Data = Data;
    }

    #region  OK RESPONSE

    public static Response<T> OK(T data)
    {
        return new(data);
    }

    #endregion

    #region  ERROR RESPONSE

    public static Response<T> ValidationError(ValidationResult validationResult)
    {
        Response<T> response = new();
        foreach(var error in validationResult.Errors)
            response.Errors.Add(ErrorMessage.Create(error.ErrorMessage));
                
        return response;
    }



    public static Response<T> Error(Exception ex)
    {
        Response<T> response = new();
        response.Errors.Add(ErrorMessage.Create(ex.GetAllExceptions()));
        
        return response;
    }

    public static Response<T> Error(Error<T> error) 
    {
        Response<T> response = new();
        response.Errors.Add(ErrorMessage.Create(error.Message,error.Code));
        
        return response;
    }
    public static Response<T> Error(object error, object ReferenceObject = null) 
    {
        Response<T> response = new();
        response.Errors.Add(ErrorMessage.Create(error.GetString("Message"), error.GetString("Code"), "","", ReferenceObject));
        
        return response;
    }

    public static Response<T> Error(string Message, string Code = null, object ReferenceObject = null)
    {
        Response<T> response = new();
        response.Errors.Add(ErrorMessage.Create(Message, Code, "","", ReferenceObject));        
        return response;
    }

    #endregion



}