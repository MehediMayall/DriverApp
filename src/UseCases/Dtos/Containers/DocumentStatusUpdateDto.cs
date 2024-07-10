namespace UseCases.Dtos;

public record DocumentStatusUpdateDto(
    DriverID driverID, 
    OrderLogID orderLogID, 
    ProNumber proNumber, 
    LegType legType, 
    Boolean IsResubmit
);