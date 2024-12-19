using Microsoft.EntityFrameworkCore;
using WordToPdfExporter.Core.Exporting;

namespace WordToPdfExporter.Infrastructure.Data;

public class WordToPdfExporterContext : DbContext
{
    public DbSet<DocumentExport> DocumentExportRequests { get; set; }

    public WordToPdfExporterContext(DbContextOptions<WordToPdfExporterContext> options) : base(options)
    {

    }
}