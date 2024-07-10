
namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class Address
{
    public int AddressId { get; set; }
    public string Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? ContactName { get; set; }
    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
    public string? CellPhone { get; set; }
    public string? Email { get; set; }
    public string? Fax1 { get; set; }
    public string? Fax2 { get; set; }
    public int? ZipCodeId { get; set; }
    public ZipCode? ZipCode { get; set; }
}
