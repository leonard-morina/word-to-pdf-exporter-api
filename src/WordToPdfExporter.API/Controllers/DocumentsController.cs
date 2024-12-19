using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WordToPdfExporter.Core.Exporting;

namespace WordToPdfExporter.API.Controllers;


[Microsoft.AspNetCore.Components.Route("api/documents")]
[AllowAnonymous]
public class DocumentsController(IDocumentExportService documentExportService) : Controller
{
    [HttpPost("export")]
    public async Task<IActionResult> ExportDocumentAsync([FromBody]ExportDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        var bytes = await documentExportService.ExportDocumentToPDFAsync(request, cancellationToken);

        var pdfFileName = (request.FileName ?? Guid.NewGuid().ToString()).Replace(".docx", ".pdf")
            .Replace(".doc", ".pdf");

        return File(bytes, "application/pdf", pdfFileName);
    }
}