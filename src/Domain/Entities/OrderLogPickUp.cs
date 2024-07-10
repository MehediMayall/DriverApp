namespace Domain.Entities;

public class OrderLogPickUp
{
    [Key]
    public int OrderLogPickUpId { get; set; }
    public int ProNumber { get;set; }
    public string? ChassisN { get; set; }
    public int? ChassisId { get; set; }
    public DateTime? WorkDatePU { get; set; }
    public int? PickUpDriverId { get; set; }
    public string? DropOnPickUp { get; set; }
    public double? TotalWeightPickUp { get;set; }
    public string? TruckNoPickedUp { get; set; }
    public bool? IsAttemptedPU { get; set;}
    public bool? IsNightPu { get; set; }
    public bool? IsPrinted { get; set; }
    public DateTime? ScheduledPickUpDate { get; set;}
    public TimeSpan? ScheduledPickUpTime { get; set; }
    public TimeSpan? TimePU  { get;set; }
    public DateTime? LastUpdateDateTime  { get; set; }
    public string? LastUpdateUserId { get; set;}
    public TimeSpan? TimeGateEquipment { get; set; }
    public DateTime? DateGateEquipment { get; set; }
    public char? PayDueScalePU { get; set; }
    public DateTime? PWorkDatePU { get; set; }

}
