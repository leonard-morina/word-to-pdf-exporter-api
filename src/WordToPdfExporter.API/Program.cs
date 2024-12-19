using Microsoft.EntityFrameworkCore;
using WordToPdfExporter.Core.Common;
using WordToPdfExporter.Core.Exporting;
using WordToPdfExporter.Core.Lock;
using WordToPdfExporter.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var databasePath = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<WordToPdfExporterContext>((options) =>
{
    options.UseSqlite($"Data Source={databasePath}");
});
builder.Services.AddSingleton<ILockService>(provider => new FileLockService(@"C:\temp\word-to-pdf.lock"));

builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(WordToPdfExporterRepository<>));
builder.Services.AddScoped<IDocumentExportService, DocumentExportService>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WordToPdfExporterContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
