using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Word;
using WordToPdfExporter.Core.Common;

namespace WordToPdfExporter.Core.Exporting;

public class DocumentExportService(
    IAsyncRepository<DocumentExport> documentExportRequestRepository,
    IAsyncRepository<CurrentDocumentExport> currentDocumentExportRepository) : IDocumentExportService
{
    public Task<byte[]> ExportDocumentToPDFAsync(ExportDocumentRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var documentAsBytes = Convert.FromBase64String(request.Base64);

            var tempWordFilePath = Path.Combine(Path.GetTempPath(), "tempDocument.docx");
            File.WriteAllBytes(tempWordFilePath, documentAsBytes);

            var wordApp = new Application();
            var wordDocument = wordApp.Documents.Open(tempWordFilePath);

            var tempPdfFilePath = Path.Combine(Path.GetTempPath(), "tempDocument.pdf");

            wordDocument.ExportAsFixedFormat(
                tempPdfFilePath,
                WdExportFormat.wdExportFormatPDF,
                OpenAfterExport: false);
            wordDocument.Close();
            wordApp.Quit();

            var pdfBytes = File.ReadAllBytesAsync(tempPdfFilePath, cancellationToken);

            File.Delete(tempWordFilePath);
            File.Delete(tempPdfFilePath);

            return pdfBytes;
        }
        catch (COMException ex)
        {
            throw new Exception($"Cannot process document: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Cannot process document: {ex.Message}");
        }
    }
}