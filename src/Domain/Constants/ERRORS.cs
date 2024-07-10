namespace Domain.Constants;

public abstract class ERRORS
{
    public const string ARGUMENT_OBJECT_NULL = "Input validation failed. Parameter object is null or invalid. Please try again.";
    public const string DATA_OBJECT_NULL = "Data object is null. Couldn't find any data and returned null object. Please try again.";
    public const string SESSION_USER_NOT_FOUND = "Session user not found. Please login and try again.";
    public const string INVALID_CREDENTIALS = "Invalid Credentials. Please try again.";
    public const string INCORRECT_USERNAME = "Username or password is incorrect.";
    public const string FAILED_TO_GENERATE_TOKEN = "User validated but failed to generate token. Please try again.";
    public const string INVALID_USER_TYPE = "Invalid user type.";
    public const string INVALID_SIGNATURE_IMAGE = "Invalid signature image. Please upload a valid image in base64";
    public const string INVALID_IMAGE = "Didn't find any image. Please upload a valid image in base64";
    public const string INVALID_DOMAIN_NAME = "Invalid domain name or couldn't find the api domain address in the appsettings.";
    public const string FAILED_DESERIALIZE_OBJECT = "Failed to deserialize object. Desrialize object is found null. Please try again";
     
    
}
