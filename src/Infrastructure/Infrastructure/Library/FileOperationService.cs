namespace Infrastructure.Infrastructure;

public  class FileOperationService : IFileOperationService
{

    
    public string GetFileWithoutExtension(string filename) => filename.Replace(".pdf", "");

    public Boolean ReadFile(ref string SourceString, string FileName)
    {
        try
        {
            StreamReader sr = new StreamReader(FileName);
            SourceString = sr.ReadToEnd();
            sr.Close();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public string ReadFile(string FileName)
    {

        StreamReader sr = new StreamReader(FileName);
        var SourceString = sr.ReadToEnd();
        sr.Close();
        return SourceString;
    }

    public Boolean WriteFile(ref string SourceString, string FileName)
    {
        StreamWriter sw = new StreamWriter(FileName);
        sw.Write(SourceString);
        sw.Close();

        return true;
    }

    public Boolean DeleteFile(string FileName)
    {

        if (File.Exists(FileName))
        {
            File.Delete(FileName);
            return true;
        }
        else return true;
    }

    public Boolean CheckAndCreateDir(string Dir)
    {
        try
        {
            Directory.CreateDirectory(Dir);
            if (Directory.Exists(Dir)) return true;
        }
        catch { }


        string[] dr = Dir.Split('\\');
        string curDir = "";

        foreach (string st in dr)
        {

            if (string.IsNullOrEmpty(st) || st == "\\" || st == @"\") continue;

            if (curDir.Length == 0) curDir = st;
            else curDir = curDir + "\\" + st;
            DirectoryInfo di = new DirectoryInfo(curDir);
            if (di.Exists == false)
            {
                di.Create();
            }
        }

        return true;

    }


    public Boolean WriteFile(ref string SourceString, string AbsolutePath, Boolean Append)
    {
        try
        {
            StreamWriter sw = new StreamWriter(AbsolutePath, Append);
            sw.Write(SourceString);
            sw.Close();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

}

