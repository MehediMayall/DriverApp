namespace Domain.ModelsCustom;

public class DriverContainerModel
{
	public int? OrderLogID { get; set; }
	[Key]
	public Nullable<int> Pro { get; set; }
	public string? ContainerCode { get; set; }
	public string? AppointmentTime { get; set; }
	public string? MTPickupDate { get; set; }
	public string? DestinationCity { get; set; }
	public string? DestinationState { get; set; }

	public string? LoadType { get; set; }
	public string? Container { get; set; }
	public string? PO { get; set; }
	public Nullable<double> Quantity { get; set; }
	public string? PDiemDate { get; set; }

	public string? ScheduledDeliveryDate { get; set; }
	public string? ContainerSize { get; set; }
	public string? FreeTime { get; set; }
	public string? PickupDate { get; set; }
	public string? DeliveryDate { get; set; }

	public string? ReturnDate { get; set; }
	public Nullable<int> PAccount { get; set; }
	public string? ETA { get; set; }
	public string? PConsignee { get; set; }
	public string? Consignee { get; set; }

	public string? Terminal { get; set; }
	public string? WorkDate { get; set; }
	public string? AvailableDate { get; set; }
	public string? DateMT { get; set; }
	public string? ArrivedOn { get; set; }

	public string? OrderStatus { get; set; }
	public Nullable<double> Weight { get; set; }
	public string? WeightUnit { get; set; }
	public string? WorkDateDelivery { get; set; }
	public Nullable<DateTime> OrderEntryDate { get; set; }

	public string? LegType { get; set; }
	public string? ViewMovedOn { get; set; }
	public string? BeginMovedOn { get; set; }
	public Nullable<int> MoveStatusID { get; set; }
	public string? SpecialInstructions { get; set; }

	public string? ConsigneeCity { get; set; }
	public string? ConsigneeAddress { get; set; }
	public string? ConsigneeState { get; set; }
	public string? ConsigneeZip { get; set; }
	public string? TerminalCity { get; set; }

	public string? TerminalAddress { get; set; }
	public string? TermianalState { get; set; }
	public string? TerminalZip { get; set; }
	public string? WarehouseCity { get; set; }
	public string? WarehouseAddress { get; set; }

	public string? WarehouseState { get; set; }
	public string? WarehouseZip { get; set; }
	public string? Warehouse { get; set; }
	public string? DropOnDelivery { get; set; }
	public string? DropOnReturn { get; set; }

	public string? DropOnPickup { get; set; }
	public string? ChassisNumber { get; set; }
	public string? LineChassis { get; set; }
	public string? Commodity { get; set; }
	public string? Seal { get; set; }

	public string? OtherRef { get; set; }
	public string? PickupTerminal { get; set; }
	public string? ReturnTerminal { get; set; }
	public string? VesselName { get; set; }
	public string? BillOrBooking { get; set; }

	public string? PinOut { get; set; }
	public string? PinIn { get; set; }
	public string? WarehousePhone { get; set; }
	public string? ConsigneePhone { get; set; }
	public string? ConsigneeContact { get; set; }

	public string? Imcha { get; set; }
	public string? OceanCarrier { get; set; }
	public Nullable<int> PickupNumber { get; set; }
	public Nullable<int> Returnmtnum { get; set; }
	public Nullable<DateTime> VesselCutOffDate { get; set; }
	public string? ContainerType { get; set; }
	public Nullable<Boolean> IsBannered { get; set; }
	public string? ApptDate { get; set; }
	public string? PURailOrWarehouse { get; set; }
	public string? DriverPickup { get; set; }
	public string? DriverDelivery { get; set; }
	public string? COMPANYNAME { get; set; }
	public Nullable<Boolean> IsPaperLess { get; set; }
	public string? ConsigneePODEmail { get; set; }
	public string? WarehousePODEmail { get; set; }
	public string? PickupApt { get; set; }
	public string? ReturnApt { get; set; }
	public string? EntryNo { get; set; }

	public Nullable<int> IsDriverInBeginMove { get; set; }
}
