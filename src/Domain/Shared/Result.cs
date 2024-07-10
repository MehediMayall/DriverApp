using Domain.Exceptions;

namespace Domain.Shared;

public class Result<T> where T : class
{
    private Result(bool isSuccess, Error<T> error, T value = null)
    {
        if (isSuccess && error != Error<T>.None || 
            !isSuccess && error == Error<T>.None)
        {
            throw new ArgumentException("Invalid Error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
        Value = value;        
    }

    public bool IsSuccess {get;}
    public bool IsFailure => !IsSuccess;
    public Error<T> Error {get;}
    public T Value {get;}

    public static Result<T> Success(T value) => new(true, Error<T>.None, value);
    public static Result<T> Failure(Error<T> error) => new(false, error);
}