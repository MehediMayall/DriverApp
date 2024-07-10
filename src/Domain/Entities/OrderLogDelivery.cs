namespace Domain.Entities;

public class OrderLogDelivery
{
    [Key]
    public int OrderLogDeliveryId { get; set; }
    public int ProNumber { get; set; }
    public int? DeliveryDriverId { get; set; }
    public DateTime? ScheduledDeliveryDate { get; set; }
    public string? DropOnDelivery { get; set; }
    public double? TotalWeightDelivery { get; set; }
    public string? TruckNoDelivery { get; set; }
    public bool? IsAttemptedDel { get; set; }
    public bool? IsNightDel { get; set; }
    public DateTime? WorkDelDate { get; set; }
    public TimeSpan? WorkDelTime { get; set; }
    public TimeSpan? ScheduledDelApptTime { get; set; }
    public DateTime? LastUpdateDateTime { get; set; }
    public string? LastUpdateUserId { get; set; }
    public TimeSpan? TimeGateLoad { get; set; }
    public DateTime? DateGateLoad { get; set; }
    public char? PayDueScaleDel { get; set; }
    public DateTime? PWorkDelDate { get; set; }


}
