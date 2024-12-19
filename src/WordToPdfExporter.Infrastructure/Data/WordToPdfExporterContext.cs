using Microsoft.EntityFrameworkCore;
using WordToPdfExporter.Core.Exporting;

namespace WordToPdfExporter.Infrastructure.Data;

public class WordToPdfExporterContext : DbContext
{
    public DbSet<DocumentExport> DocumentExports { get; set; }

    public WordToPdfExporterContext(DbContextOptions<WordToPdfExporterContext> options) : base(options)
    {
    }
}