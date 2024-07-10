namespace API.Tests.MockData;

public class WorkQueueMockData: UserMockData
{

    public string Data_RootDir = "";

    public WorkQueueMockData()
    {
        Data_RootDir = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString(),"MockData");
        Data_RootDir = Path.Combine(Data_RootDir,"Data");

    }

    public string GetWorkQueuesDataInString()
    {
        using(StreamReader sr = new StreamReader(Path.Combine(Data_RootDir, "WorkQueue.json")))
        {
            return sr.ReadToEnd();
        }
    }
    public string GetWorkQueueDataInString()
    {
        return JsonSerializer.Serialize(GetWorkQueues().FirstOrDefault());
    }

    public List<WorkQueueModel> GetWorkQueues()
    {
        return JsonSerializer.Deserialize<List<WorkQueueModel>>(GetWorkQueuesDataInString());
    }

    public WorkQueueModel GetWorkQueue()
    {
        return JsonSerializer.Deserialize<WorkQueueModel>(GetWorkQueueDataInString());
    }

    

}