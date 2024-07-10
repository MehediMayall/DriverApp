
namespace UseCases.Dtos;

public sealed record SessionUserDto(UserID userID, DriverID driverID, CompanyID companyID, NonEmptyString DriverName, string Division="AB");