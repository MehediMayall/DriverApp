namespace Domain.Exceptions;

public static class PurchaseOrderError<PurchaseOrder> where PurchaseOrder : class
{     
    public static Error<PurchaseOrder> PurchaseOrderNotFound(int ProNumber) => 
        new($"Couldn't find any purchase order data for ProNumber: {ProNumber}", "ContainerRepository.GetPurchaseOrder");
    
}