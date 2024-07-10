namespace Domain.ModelsCustom;
public class OrderLogDocument
{
 
    public int? CompanyId { get; set; }

    public Nullable<int> DocId { get; set; }
    public string? DocNamePro { get; set; }
    public Nullable<int> ProNumber { get; set; }
    public Nullable<int> OrderLogId { get; set; }
    public string? SubFolder { get; set; }

}
