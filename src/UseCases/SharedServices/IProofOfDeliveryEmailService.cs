namespace UseCases.SharedServices;

public interface IProofOfDeliveryEmailService
{
    Task<Result<string>> SendPODASEmail(DriverContainerModel container, string PODFilePath);
}