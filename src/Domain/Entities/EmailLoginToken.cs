using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Email_LoginToken")]
public partial class EmailLoginToken
{
    [Key]
    public int Id { get; set; }
    public int? CompanyId { get; set; }
    public string? ContainerCode { get; set; }
    public DateTime? ValidTill { get; set; }
    public bool? IsUserLoggedOn { get; set; }
    public int? DriverId { get; set; }
    public DateTime? CreatedOn { get; set; }
}
