namespace WordToPdfExporter.Core.Exporting;

public interface IDocumentExportService
{
    Task<byte[]> ExportDocumentToPDFAsync(ExportDocumentRequest request, CancellationToken cancellationToken = default);
}