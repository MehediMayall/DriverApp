namespace Domain.Entities;

public class OrderLogFreightCustoms
{
    [Key]
    public int OrderLogFreightCustomsId { get; set; }
    public int ProNumber { get; set; }
    public DateTime? NoFreightDate1 { get; set; }
    public DateTime? NoFreightDate2 { get; set; }
    public DateTime? NoFreightDate3 { get; set; }
    public DateTime? NoFreightDate4 { get; set; }
    public DateTime? NoFreightDate5 { get; set; }
    public DateTime? NoFreightDate6 { get; set; }
    public DateTime? NoCustoms1 { get; set; }
    public DateTime? NoCustoms2 { get; set; }
    public DateTime? NoCustoms3 { get; set; }
    public DateTime? NoCustoms4 { get; set; }
    public DateTime? NoCustoms5 { get; set; }
    public DateTime? NoCustoms6 { get; set; }
    public DateTime? FreeTimeDate { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public DateTime? AvailableDate { get; set; }
    public DateTime? HeldForExamDate { get; set; }
    public DateTime? StopOffDate { get; set; }
    public int? PinIn { get; set; }
    public int? PinOut { get; set; }
}
