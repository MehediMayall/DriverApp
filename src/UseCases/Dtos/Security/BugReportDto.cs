namespace UseCases.Dtos;

[ExcludeFromCodeCoverage]
public record BugReportDto(string code, string url, string title, string details, string buglevelid);
