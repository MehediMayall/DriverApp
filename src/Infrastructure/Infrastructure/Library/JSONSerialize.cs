using Newtonsoft.Json;
using System;
using System.Text;


namespace Infrastructure.Library;
public class JSONSerialize
{
    public static string ErrorMessage = "";
    public Exception Error = null;



    public T getObjectFromJSONString<T>(string JSONString)
    {
        return JsonConvert.DeserializeObject<T>(JSONString);
    }



    public List<T> getJSON<T>(string JSONString)
    {
        return JsonConvert.DeserializeObject<List<T>>(JSONString);
    }

    public string getJSONString(object obj)
    {
        try
        {
            ErrorMessage = "";
            return JsonConvert.SerializeObject(obj);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.GetAllExceptions();
            return null;
        }
    }

    public string getJSONString(object obj, Boolean IgnoreReferenceLoopHandling)
    {
        try
        {
            ErrorMessage = "";

            if (IgnoreReferenceLoopHandling)
            {
                return JsonConvert.SerializeObject(obj, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                        });
            }
            else return JsonConvert.SerializeObject(obj);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.GetAllExceptions();
            return null;
        }
    }





    public string EncodeBase64(string plainText)
    {
        ErrorMessage = "";
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    public string DecodeBase64(string EncodedString)
    {
        try
        {
            ErrorMessage = "";
            byte[] data = Convert.FromBase64String(EncodedString);
            return Encoding.UTF8.GetString(data);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.GetAllExceptions();
            return null;
        }
    }
}
