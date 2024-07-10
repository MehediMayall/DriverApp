

namespace Domain.Entities;
public class GlobalSetup 
{
    [Key]
    public int? GlobalSetupId { get; set; }

    public string? CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyAddress { get; set; }
    public string? CompanyPhone { get; set; }
    public string? CompanyFax { get; set; }

    public string? ScannedDocPath { get; set; }
    public string? ProgramTitle { get; set; }
    public string? CompanyCode { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }

    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? LocalScannedDocPath { get; set; }
}
