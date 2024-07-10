namespace UseCases.Extensions;

public static class ExceptionMessageExtensionMethods
{
    public static string GetAllExceptions(this Exception ex)
    {
        try
        {
            var (Source, Message) = (ex?.Source ?? string.Empty, ex?.Message ?? string.Empty);
            StringBuilder ErrorDetail = new();
            ErrorDetail.Append($"Source: {Source} ==> {Message}.");


            // Inner Exception level 1
            if (ex!.InnerException != null)
            {
                var iEx = ex.InnerException;
                ErrorDetail.Append($"Source: {iEx!.Source ?? string.Empty} ==> {iEx!.Message ?? string.Empty}");

                // Inner Exception level 2
                if (iEx!.InnerException != null)
                {
                    var iEx2 = iEx.InnerException;
                    ErrorDetail.Append($"Source: {iEx2!.Source ?? string.Empty} ==> {iEx2!.Message ?? string.Empty}");
                }
            }
            return ErrorDetail.ToString();
        }
        catch(Exception exception){ 
            return "Unexpted error occured in GetErrorMessage. Exception detail: " + exception.Message; 
        }
    }
}