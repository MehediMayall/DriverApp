namespace  API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BugReportingController 
{

    [AllowAnonymous]
    [HttpPost("/api/bug/reporting/send")]
    public async Task<Response<string>> BugReportingSend([FromBody] BugReportDto BugReport)
    {
        if (BugReport.buglevelid.ToUpper() == "WARNING") Log.Warning($"APP_ERROR: {BugReport.details}, Title: {BugReport.title}, Code: {BugReport.code}, URL: {BugReport.url}");
        else if (BugReport.buglevelid.ToUpper() == "INFO") Log.Information($"APP_ERROR: {BugReport.details}, Title: {BugReport.title}, Code: {BugReport.code}, URL: {BugReport.url}");
        else if (BugReport.buglevelid.ToUpper() == "CRITICAL") Log.Fatal($"APP_ERROR: {BugReport.details}, Title: {BugReport.title}, Code: {BugReport.code}, URL: {BugReport.url}");
        else Log.Error($"APP_ERROR: {BugReport.details}, Title: {BugReport.title}, Code: {BugReport.code}, URL: {BugReport.url}");

        return Response<string>.OK("Successfully saved");
    }
}
