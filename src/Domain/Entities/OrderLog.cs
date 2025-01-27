﻿namespace Domain.Entities;

public class OrderLog
{
    [Key]
    public int ProNumber { get; set; }
    public DateTime? CreationDateTime { get; set; }
    public string? CreationUserId { get; set; }
    public DateTime? LastUpdateDateTime { get; set; }
    public string? LastUpdateUserId { get; set; }
    public string? CompanyId { get; set; }
    public DateTime? DeliveryOrderReceivedOn { get; set; }
    public int? CarrierId { get; set; }
    public string? ContainerCode { get; set; }
    public string? TrailerNo { get; set; }
    public string? ContainerType { get; set; }
    public string? ContainerSize { get; set; }
    public DateTime? PickedUpDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string? SealNumber { get; set; }
    public int? BillToCustomerId { get; set; }
    public int? VesselId { get; set; }
    public DateTime? ExpectedTimeOfArrival { get; set; }
    public double? Quantity { get; set; }
    public string? Commodity { get; set; }
    public string? CommodityCategory { get; set; }
    public int? ConsigneeId { get; set; }
    public string? PurchaseOrder { get; set; }
    public string? LoadType { get; set; }
    public string? DeliveryOrderFlagged { get; set; }
    public int? TerminalId { get; set; }
    public DateTime? Dat { get; set; }
    public string? Billed { get; set; }
    public string? BillOfLading { get; set; }
    public DateTime? ScheduledDateTime { get; set; }
    public int? WarehouseId { get; set; }
    public double? Weight { get; set; }
    public string? WeightUnit { get; set; }
    public double? EquWeight { get; set; }
    public string? DestinationCity { get; set; }
    public string? DestinationState { get; set; }
    public DateTime? FreeTime { get; set; }
    public string? ApptTtm9 { get; set; }
    public string? c_t { get; set; }
    public string? ana_s_note { get; set; }
    public DateTime? DateNotified { get; set; }
    public double? InvoiceAmount { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public double? RateAmount { get; set; }
    public double? Fs { get; set; }
    public double? TotalAmount { get; set; }
    public string? Done { get; set; }
    public double? Overweight { get; set; }
    public string? NjOwPermit { get; set; }
    public double? Detention { get; set; }
    public int? RateId { get; set; }
    public string? LineChassis { get; set; }
    public string? RoundTrip { get; set; }
    public string? OtherRef { get; set; }
    public string? EntryNumber { get; set; }
    public int? CustomsBrokerId { get; set; }
    public DateTime? DateArrivedOn { get; set; }
    public int? ShipperId { get; set; }
    public double? ChassisWeight { get; set; }
    public string? PuNumber { get; set; }
    public string? Dispatcher { get; set; }
    public string? FullMt { get; set; }
    public string? ActualArrivalTime { get; set; }
    public string? BLnote { get; set; }
    public string? YD { get; set; }
    public string? PD { get; set; }
    public string? M1 { get; set; }
    public string? M2 { get; set; }
    public string? CC { get; set; }
    public double? ChaChg { get; set; }
    public double? Pdimes { get; set; }
    public string? ProCheck { get; set; }
    public string? RatedUserId { get; set; }
    public bool IsLocked { get; set; }
    public string? PdiemNo { get; set; }
    public string? MultiPRO { get; set; }
    public string? FwPriority { get; set; }
    public DateTime? FwDate { get; set; }        
    public string? OldSeal { get; set; }
    public bool IsAgentComPosted { get; set; }
    public bool IsNoSpLocChg { get; set; }
    public bool IsBroker { get; set; }
    public int? DoorId { get; set; }
    public string? FuelChgType { get; set; }
    public double? FsFlatAmt { get; set; }
    public int HwtiPro { get; set; }
    public int HwtiProRev { get; set; }
    public bool IsBonded { get; set; }
    public bool IsHot { get; set; }
    public bool IsOWCorridor { get; set; }
    public string? Tariff { get; set; }
    public bool IsChaChg { get; set; }
    public bool IsMultipleRate { get; set; }
    public int? MultiRateCnt { get; set; }
    public double? FwQty { get; set; }
    public bool IsCrossdock { get; set; }
    public bool IsPDiemIgnore { get; set; }
    public DateTime? ActualMtDate { get; set; }
    public OrderLogFreightCustoms? OrderLogLogFreightCustoms { get; set; }
    public OrderLogDelivery? OrderLogDelivery { get; set; }
    public OrderLogPickUp? OrderLogPickUp { get; set; }
    public OrderLogPickupMT? OrderLogPickupMT { get; set; }
    public OrderLogReturnMT? OrderLogReturnMT { get; set; }
    public DateTime? VesselCutoffDate { get; set; }
    public string? PodPoL { get; set; }
}
