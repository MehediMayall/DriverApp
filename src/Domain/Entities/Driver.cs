namespace Domain.Entities;


public class Driver
{
    [Key]
    public int DriverId { get; set; }
    public string? CompanyId { get; set; }
    public string? DriverCodeId { get; set; } 
    public string? PayrollId { get; set; }
    public string? DriverType { get; set; }
    public int? DriverCompanyId { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public int? AddressId { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? SsnNo { get; set; }
    public string? FederalId { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? TerDate { get; set; }
    public string? LicNo { get; set; }
    public string? LicState { get; set; }
    public DateTime? LicExpDate { get; set; }
    public DateTime? NextExamDate { get; set; }
    public string? AuthorStates { get; set; }
    public string? InsCompanyName { get; set; }
    public string? InsId { get; set; }
    public string? InsType { get; set; }
    public string? Status { get; set; }
    public string? Note { get; set; }
    public int? TruckId { get; set; }
    public string? Type { get; set; }
    public string? LicPlate { get; set; }
    public double? Uniform { get; set; }
    public double? WComp { get; set; }
    public double? LoanRepyt { get; set; }
    public double? EarningsAdj { get; set; }
    public bool? PayExclude { get; set; }
    public string? DriverBelong { get; set; }
    public int? DriverHwId { get; set; }
    public DateTime? OoInspDate { get; set; }
    public string? UserId { get; set; }
    public string? Password { get; set; }
    public string? Password2 { get; set; }
    public bool? IsPaperless { get; set; }
    public int? FmsDriverId { get; set; }
    public Address? Address { get; set; }
    public DateTime? CreationDateTime { get; set; }
    public string CreationUserId { get; set; } = string.Empty;
    public DateTime? LastUpdateDatetime { get; set; }
    public string? LastUpdateUserId { get; set; }
    public string? DayNight { get; set; }
    public double? Insurance { get; set; }
    public double? DirectDep { get; set; }
    public double? InsurOccAcc { get; set; }
    public double? EldSecDeposit { get; set; }
    public double? EldUsageFee { get; set; }
    public double? NTLInsur { get; set; }
    public DateTime? BobTailsAndDue { get; set; }
    public DateTime? RegDue { get; set; }
    public DateTime? LiabilityInsDue { get; set; }   
    
}
