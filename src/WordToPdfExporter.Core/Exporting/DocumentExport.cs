using System.ComponentModel.DataAnnotations;
using WordToPdfExporter.Core.Common;

namespace WordToPdfExporter.Core.Exporting;

public class DocumentExport : IEntity
{
    [Key]
    public int DocumentExportRequestId { get; set; }
    public string? RequestHost { get; set; }
    public string? RequestBody { get; set; }
    public DateTime RequestedOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
}