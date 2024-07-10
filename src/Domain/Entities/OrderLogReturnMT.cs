namespace Domain.Entities;

public class OrderLogReturnMT
{
    [Key]
    public int OrderLogReturnMTId { get; set; }
    public int ProNumber { get; set; }
    public DateTime? WorkDateReturn { get; set; }
    public DateTime? PdiemDate { get; set; }
    public int? ReturnDriverId { get; set; }
    public string? ChassisReturned { get; set; }
    public int? TerminalRetLocId { get; set; }
    public double? TotalWeightReturn { get; set; }
    public string? TruckNoReturn { get; set; }
    public bool? IsAttemptedRet { get; set; }
    public bool? IsNightRetMt { get; set; }
    public TimeSpan? ReturnTime { get; set; }
    public DateTime? ScheduledReturnDate { get; set; }
    public TimeSpan? ScheduledReturnTime { get; set; }
    public DateTime? LastUpdateDateTime { get; set; }
    public string? LastUpdateUserId { get; set; }
    public TimeSpan? TimeGateEmpty { get; set; }
    public DateTime? DateGateEmpty { get; set; }
    public char? PayDueScaleRet { get; set; }
    public DateTime? PWorkReturnDate { get; set; }


}
