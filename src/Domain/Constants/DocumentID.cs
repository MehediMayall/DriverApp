namespace Domain.Constants;
public sealed class DocumentIDs
{
    public static readonly DocumentID PROOF_OF_DELIVERY =  new DocumentID((NonNegative) 6);
    public static readonly DocumentID OUTBOUND =  new DocumentID((NonNegative) 8);
    public static readonly DocumentID INBOUND =  new DocumentID((NonNegative) 10);
    public static readonly DocumentID BILL_OF_LADING = new DocumentID((NonNegative) 5);
}

