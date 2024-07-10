using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Driver_Document_Notifications")]
public partial class DocumentNotifications
{
    [Key]
    public int Id { get; set; }
    public int DriverId { get; set; }
    public int ProNumber { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public DateTime? SeenOn { get; set; }
    public int? NotificationTypeId { get; set; }
    public DateTime CreatedOn { get; set; }
}
