namespace UseCases.Contracts.Infrastructure;

public interface IPDFService
{
    Task<string> ConvertImageToPDF(string ImageInBase64, string PDFPath, DriverDocuments NewDoc);
    Task<string> GeneratePODPDF(string ReportHTMLContent, string PDFPath);
}