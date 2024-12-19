namespace WordToPdfExporter.Core.Exporting;

public class ExportDocumentRequest
{
    public string Base64 { get; set; }
    public string? FileName { get; set; }
}