
namespace UseCases.Contracts.Infrastructure;

public interface IHTMLReportService
{
    Task<Result<string>> GetPODReportHTML(DriverContainerModel container);
    Task<Result<FileInfoDto>> GetBillOfLadingReportHTML(DriverContainerModel container);
    Task<Result<BeginMoveEmailContentDto>> GetBeginMoveReportHTML(DriverContainerModel container, string LINK, GlobalSetup DivisionInfo, EmailLoginTokenDto token);
}