using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WordToPdfExporter.Core.Common;

namespace WordToPdfExporter.Core.Exporting;

public class CurrentDocumentExport : IEntity
{
    [Key]
    public int CurrentDocumentExportId { get; set; }
    public int DocumentExportRequestId { get; set; }
    public string? RequestHost { get; set; }
    public string? RequestBody { get; set; }
    public DateTime RequestedOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
    [ForeignKey("DocumentExportRequestId")]
    public DocumentExport? DocumentExportRequest { get; set; }
}