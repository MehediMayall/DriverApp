namespace Domain.ModelsCustom;


[Keyless]
public class WorkQueueModel
{
	
	public Nullable<int> OrderLogID { get; set; }
	public Nullable<int> Pro { get; set; }

	public string? ContainerCode { get; set; }

	public Nullable<DateTime> WorkDatePickup { get; set; }
	public Nullable<DateTime> PickupDate { get; set; }
	public Nullable<DateTime> DeliveryDate { get; set; }
	public Nullable<DateTime> ReturnDate { get; set; }

	public Nullable<DateTime> WorkDateDelivery { get; set; }
	public Nullable<DateTime> OrderEntryDate { get; set; }
	public string? LegType { get; set; }
	public Nullable<DateTime> ViewMovedOn { get; set; }
	public Nullable<DateTime> BeginMovedOn { get; set; }

	public Nullable<int> MoveStatusID { get; set; }
	public Nullable<int> IsRejected { get; set; }
	public string? RejectedReason { get; set; }
	public Nullable<int> DocumentID { get; set; }
	public Nullable<Boolean> IsPaperLess { get; set; }

	public Nullable<DateTime> WORKDATE { get; set; }
	public Nullable<int> OrderNo { get; set; }
}
