namespace  UseCases.Dtos;
// 
public record DocumentSubmitRequestDto(
	DriverID DriverID, 
	ProNumber ProNumber, 
	LegType LegType,
	string ReceivedBy,
	string ReceiveDate, 
	string InTime, 
	string OutTime,
	string MiscellaneousNote ,
	string ImagesInBase64, 
	Boolean IsResubmit = false
)
{
	public static DocumentSubmitRequestDto Get(DocumentSubmitRequest request)
	{
		return new DocumentSubmitRequestDto
		(
			request.DriverID.GetDriverID(),
			request.ProNumber.GetProNumber(),
			request.LegType.GetLegType(),
			request.ReceivedBy,
			request.ReceiveDate,
			request.InTime,
			request.OutTime,
			request.MiscellaneousNote,
			request.ImagesInBase64,
			request.IsResubmit
		);		
	}
}

// Primitive
public class DocumentSubmitRequest 
{
	public int ProNumber { get; set; }
	public int DriverID { get; set; }
	public string LegType { get; set; }
	public string ReceivedBy { get; set; } 

	public string ReceiveDate { get; set; }
	public string InTime { get; set; }
	public string OutTime { get; set; } 
	public string MiscellaneousNote { get; set; }
	public string ImagesInBase64 { get; set; }
	public Boolean IsResubmit  { get; set; } = false;
};

// public record DocumentSubmitRequestDto(
// 	int ProNumber, 
// 	int DriverID,
// 	string LegType,
// 	string ReceivedBy, 
// 	Nullable<DateTime> ReceiveDate,
// 	string InTime,
// 	string OutTime, 
// 	string MiscellaneousNote,
// 	string ImagesInBase64,
// 	Boolean IsResubmit = false 
// );

