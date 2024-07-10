namespace Domain.Entities;

public class OrderLogPickupMT
{
    [Key]
    public int OrderLogPickUpMTId {get; set;}
    public int ProNumber {get; set;}
    public DateTime? PickUpDateMT {get; set;}
    public int? DriverMtId {get; set;}
    public string? TruckNoMt {get; set;}
    public string? DropOnReturn {get; set;}
    public double? TotalWeightMt {get; set;}
    public bool? IsNightPuMt {get; set;}
    public bool? IsAttemptedMt {get; set;}
    public bool? IsEmpty {get; set;}
    public TimeSpan? PuMtTime { get; set;}
    public DateTime? WorkDateMt {get; set;}
    public DateTime? ScheduledPickUpdate {get; set;}
    public TimeSpan? ScheduledPickUpTime {get; set;}
    public DateTime? LastUpdateDateTime {get; set;}
    public string? LastUpdateUserId {get; set;}
    public int? PdiemNo { get; set; }
    public DateTime? NotifyDate { get; set; }
    public TimeSpan? NotifyTime { get; set; }
    public TimeSpan? TimeGateMt { get; set; }
    public DateTime? DateGateMt { get; set; }
    public char? PayDueScaleMT { get; set; }
    public DateTime? PWorkMtDate { get; set; }


}
