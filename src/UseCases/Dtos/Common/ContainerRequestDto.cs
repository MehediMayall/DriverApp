namespace UseCases.Dtos;
public record ContainerRequestDto(DriverID driverID, ProNumber proNumber, LegType legType)
{
    public static ContainerRequestDto Get(DriverID driverID, ContainerRequest request)
    {
        return new ContainerRequestDto(driverID, request.ProNumber.GetProNumber(), request.LegType.GetLegType());
    }
    public static ContainerRequestDto Get(DriverID driverID, int ProNumber, string LegType)
    {
        return new ContainerRequestDto(driverID, ProNumber.GetProNumber(), LegType.GetLegType());
    }
}
public record ContainerRequest(int ProNumber, string? LegType);

