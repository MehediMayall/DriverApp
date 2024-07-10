namespace Domain.Exceptions;

public static class GlobalSetupError<GlobalSetup> where GlobalSetup : class
{     
    public static Error<GlobalSetup> GlobalSetupNotFound(CompanyID companyID)
    {
        return new($"Couldn't find any globalsetup data for company id: {companyID.Value}", "ContainerRepository.GetDivisionInfo");
    }
}