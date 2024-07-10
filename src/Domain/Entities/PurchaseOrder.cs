namespace Domain.Entities;

public class PurchaseOrder
{
    [Key]
    public Nullable<int> PurchaseOrderId { get; set; }

    public Nullable<DateTime> CreationDateTime { get; set; }
    public string CreationUserId { get; set; }
    public Nullable<DateTime> LastUpdateDateTime { get; set; }
    public string LastUpdateUserId { get; set; }
    public string PurchaseOrderNo { get; set; }

    public Nullable<int> ProNumber { get; set; }
    public string Type { get; set; }
    public Nullable<int> KeySequence { get; set; }
    public string companyid { get; set; }
}
