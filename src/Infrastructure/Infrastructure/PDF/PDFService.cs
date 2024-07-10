namespace Infrastructure.Infrastructure;

public class PDFService: IPDFService
{
    private readonly IFileOperationService fileOperationService;
    private readonly IConverter converter;
    private readonly IImageService imageService;

    public PDFService(IFileOperationService fileOperationService, IConverter converter, IImageService imageService)
    {
        this.fileOperationService = fileOperationService;
        this.converter = converter;
        this.imageService = imageService;
    }



    public async Task<string> ConvertImageToPDF(string ImageInBase64, string PDFPath, DriverDocuments NewDoc)
    {
        string pdfFileName = PDFPath;


        var globalSettings = new GlobalSettings
        {
            ColorMode = DinkToPdf.ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4,
            Margins = new MarginSettings { Top = 10 },
            DocumentTitle = "Inbound TIR",
            Out = pdfFileName
        };

        string ReceivedBy = "";
        string InTime = "";
        string OutTime = "";

        if (NewDoc != null)
        {
            ReceivedBy = NewDoc.ReceivedBy;
            InTime = NewDoc.InTime;
            OutTime = NewDoc.OutTime;
        }



        ImageSize image = await imageService.GetImageSize(ImageInBase64);


        string HtmlContent = "";
        if (image != null) HtmlContent = "<!DOCTYPE html><html><body> <img style='height:" + image.Height + "px;width:" + image.Width + "px;' src='data:image/png;base64," + ImageInBase64 + "' /> </body></html>";
        else HtmlContent = "<!DOCTYPE html><html><body> <img src='data:image/png;base64," + ImageInBase64 + "' /> </body></html>";

        var objectSettings = new ObjectSettings
        {
            PagesCount = true,
            HtmlContent = HtmlContent,
            //WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
            HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
            //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
        };

        var pdf = new HtmlToPdfDocument()
        {
            GlobalSettings = globalSettings,
            Objects = { objectSettings }
        };

        converter.Convert(pdf);

        return pdfFileName;
    }

    public async Task<string> GeneratePODPDF(string ReportHTMLContent, string PDFPath)
    {
        string dir = new FileInfo(PDFPath).Directory.FullName;
        //saveLog("::: =========>  generatePODPDF -> " + PDFPath + " Parent Dir: " + dir);
        fileOperationService.CheckAndCreateDir(dir);

        if (!Directory.Exists(dir))
        {
            //saveLog("Dir: " + dir + " does not exists");
            //saveLog("ERROR WHILE CREATING DIR: " + Scripting.Error);
            throw new Exception("Error while creating POD directory. Error -> ");
        }


        var globalSettings = new GlobalSettings
        {
            ColorMode = DinkToPdf.ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4,
            Margins = new MarginSettings { Top = 10 },
            DocumentTitle = "Proof of Delivery",
            Out = PDFPath
        };

        var objectSettings = new ObjectSettings
        {
            PagesCount = true,
            HtmlContent = ReportHTMLContent, //"<!DOCTYPE html><html><head><style>table {  border-collapse: collapse;  width: 100%;}th td {  text-align: left;  padding: 8px;}tr:nth-child(even){background-color: #f2f2f2}th {  background-color: #4CAF50;  color: white;}</style></head><body><h2>Colored Table Header</h2><table>  <tr>    <th>Firstname</th>    <th>Lastname</th>    <th>Savings</th>  </tr>  <tr>    <td>Peter</td>    <td>Griffin</td>    <td>$100</td>  </tr>  <tr>    <td>Lois</td>    <td>Griffin</td>    <td>$150</td>  </tr>  <tr>    <td>Joe</td>    <td>Swanson</td>    <td>$300</td>  </tr>  <tr>    <td>Cleveland</td>    <td>Brown</td>    <td>$250</td></tr></table></body></html>",
                                             //WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
            HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
            //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
        };

        var pdf = new HtmlToPdfDocument()
        {
            GlobalSettings = globalSettings,
            Objects = { objectSettings }
        };

        converter.Convert(pdf);

        return PDFPath;
    }
}