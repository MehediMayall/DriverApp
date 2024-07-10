

namespace DriverApp.API;

public class GlobalExceptionsLogger : IMiddleware
{
    public GlobalExceptionsLogger()
    {
        
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await CreateErrorResponse(ex, context);
        }
    }

    private async Task CreateErrorResponse(Exception ex, HttpContext context)
    {
        Response<string> errorResponse =  Response<string>.Error(ex);

        // SAVE LOG
        try { Log.Error(errorResponse.Errors.FirstOrDefault().Details); } catch { }

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "Application/json";
        //string jsonString = JsonSerializer.Serialize(response);
        await context.Response.WriteAsJsonAsync(errorResponse, new JsonSerializerOptions { WriteIndented = false, PropertyNamingPolicy = null });
    }

   
}
