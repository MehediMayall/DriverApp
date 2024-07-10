namespace UseCases.SharedServices;

public interface IDeserializeService
{
    Result<T> Get<T>(string ObjectInJSONString) where T: class;
}