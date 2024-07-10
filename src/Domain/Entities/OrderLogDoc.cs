namespace Domain.Entities;

public class OrderLogDoc
{
    [Key]
    public Nullable<int> OrderLogDocId { get; set; }

    public Nullable<DateTime> CreationDateTime { get; set; }
    public string? CreationUserId { get; set; }
    public Nullable<DateTime> LastUpdateDateTime { get; set; }
    public string? LastUpdateUserId { get; set; }
    public string? CompanyId { get; set; }

    public Nullable<int> DocId { get; set; }
    public string? DocNamePro { get; set; }
    public Nullable<int> OrderLogId { get; set; }
    public string? SubFolder { get; set; }
    public Nullable<int> KeySequence { get; set; }
}
