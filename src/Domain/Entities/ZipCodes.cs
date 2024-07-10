namespace Domain.Entities;

public class ZipCode
{
    [Key]
    public int? ZipCodeId { get; set; }
    public string? Code { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? AreaCode { get; set; }
    public string? County { get; set; }
    public string? CompanyId { get; set; }
    public DateTime? CreationDateTime { get; set; }
    public string? CreationUserId { get; set; }
    public DateTime? LastUpdateDateTime { get; set; }
    public string? LastUpdateUserId { get; set; }
}
