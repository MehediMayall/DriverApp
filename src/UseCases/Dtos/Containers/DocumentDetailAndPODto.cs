namespace UseCases.Dtos;
public record DocumentDetailAndPODto(DriverContainerModel? Instructions, IReadOnlyList<PODto>? POList);

