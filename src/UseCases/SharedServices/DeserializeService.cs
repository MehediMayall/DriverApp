namespace UseCases.UseCases.SharedServices;

public class DeserializeService : IDeserializeService
{
    public Result<T> Get<T>(string ObjectInJSONString) where T: class
    {
        if (string.IsNullOrEmpty(ObjectInJSONString)) return SerializeError<T>.RequestedParameterIsNull();
        
        T? deserializedObject = ObjectInJSONString.ConvertTo<T>();
        if (deserializedObject is null) 
            return SerializeError<T>.FailedToDeserializeObject();

        return Result<T>.Success(deserializedObject);
    }
}