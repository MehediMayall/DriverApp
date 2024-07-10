using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Driver_Move")]
public class DriverMoves
{
    [Key]
    public Nullable<int> ID { get; set; }

    public string? CompanyID { get; set; }
    public Nullable<int> ProNumber { get; set; }
    public Nullable<int> DriverID { get; set; }
    public string? LegType { get; set; }
    public Nullable<DateTime> ViewMovedOn { get; set; }

    public Nullable<DateTime> BeginMovedOn { get; set; }
    public Nullable<Boolean> IsEmailSent { get; set; }
    public Nullable<DateTime> MoveCompletedOn { get; set; }
    public Nullable<DateTime> CreatedOn { get; set; }
}
