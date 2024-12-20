using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.Office.Interop.Word;
using WordToPdfExporter.Core.Common;
using WordToPdfExporter.Core.Lock;

namespace WordToPdfExporter.Core.Exporting;

public class DocumentExportService(
    IAsyncRepository<DocumentExport> documentExportRepository,
    ILockService lockService) : IDocumentExportService
{
    private readonly int _lockTimeoutMinutes = 5;

    public async Task<byte[]> ExportDocumentToPDFAsync(ExportDocumentRequest request, CancellationToken cancellationToken = default)
    {
        Document? wordDocument = null;
        Application? wordApp = null;
        byte[] pdfBytes;

        try
        {
            await lockService.WaitForUnlockAsync(TimeSpan.FromMinutes(_lockTimeoutMinutes));

            await lockService.LockAsync(TimeSpan.FromMinutes(_lockTimeoutMinutes));

            var body = JsonSerializer.Serialize(request);
            var documentExport = new DocumentExport
            {
                RequestedOn = DateTime.UtcNow,
                RequestBody = body,
            };

            await documentExportRepository.AddAsync(documentExport, cancellationToken);

            var tempDocumentName = Guid.NewGuid().ToString();
            var documentAsBytes = Convert.FromBase64String(request.Base64);

            var tempWordFilePath = Path.Combine(Path.GetTempPath(), $"{tempDocumentName}.docx");
            await File.WriteAllBytesAsync(tempWordFilePath, documentAsBytes, cancellationToken);

            wordApp = new Application();
            wordDocument = wordApp.Documents.Open(tempWordFilePath);
            
            var tempPdfFilePath = Path.Combine(Path.GetTempPath(), $"{tempDocumentName}.pdf");
            
            wordDocument.ExportAsFixedFormat(
                tempPdfFilePath,
                WdExportFormat.wdExportFormatPDF,
                OpenAfterExport: false);
            wordDocument.Close();
            wordApp.Quit();
            
            pdfBytes = await File.ReadAllBytesAsync(tempPdfFilePath, cancellationToken);
            
            File.Delete(tempWordFilePath);
            File.Delete(tempPdfFilePath);

            lockService.Unlock();
        }
        catch (COMException ex)
        {
            lockService.Unlock();
            throw new Exception($"Cannot process document: {ex.Message}");
        }
        catch (Exception ex)
        {
            lockService.Unlock();
            throw new Exception($"Cannot process document: {ex.Message}");
        }
        finally
        {
            try
            {

                // Ensure proper cleanup of Word objects
                if (wordDocument != null)
                {
                    wordDocument.Close(false);
                    Marshal.ReleaseComObject(wordDocument);
                }

                if (wordApp != null)
                {
                    wordApp.Quit();
                    Marshal.ReleaseComObject(wordApp);
                }

                // Force garbage collection to clean up remaining COM objects
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch
            {
                //ignored
            }
        }
        return pdfBytes;
    }
}