
namespace Domain.Exceptions;

public static class  PODTemplatePathError 
{     
    public static string TemplateFileNotFound(string TemplateFilePath)=>
        $"Couldn't find any driver moves for TemplateFilePath: {TemplateFilePath}";
    
    
}