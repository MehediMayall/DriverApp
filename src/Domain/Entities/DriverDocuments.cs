using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Driver_Documents")]
public partial class DriverDocuments
{
    public int Id { get; set; }
    public string? ContainerCode { get; set; }
    public int? CompanyId { get; set; }
    public int? DriverID { get; set; }
    public int? ProNumber { get; set; }
    public string? LegType { get; set; }
    public int? DocumentId { get; set; }
    public string? PhysicalFilename { get; set; }
    public bool? IsRejected { get; set; }
    public string? ReceivedBy { get; set; }
    public DateTime? ReceiveDate { get; set; }
    public string? InTime { get; set; }
    public string? OutTime { get; set; }
    public string? MiscellaneousNote { get; set; }
    public string? RejectedReason { get; set; }
    public string? Remarks { get; set; }
    public bool? IsPaperlessTerminal { get; set; }
    public bool? IsActive { get; set; }
    public int? CreatedById { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? UpdatedById { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
