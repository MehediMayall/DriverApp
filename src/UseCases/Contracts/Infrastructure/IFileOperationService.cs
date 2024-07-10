namespace UseCases.Contracts.Infrastructure;

public interface IFileOperationService
{
    string ReadFile(string FileName);
    Boolean ReadFile(ref string SourceString, string FileName);
    Boolean CheckAndCreateDir(string Dir);
    Boolean DeleteFile(string FileName);
    Boolean WriteFile(ref string SourceString, string FileName);
    string GetFileWithoutExtension(string filename);
}