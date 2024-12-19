using Microsoft.EntityFrameworkCore;
using WordToPdfExporter.Core.Common;
using WordToPdfExporter.Core.Exporting;
using WordToPdfExporter.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WordToPdfExporterContext>((options) =>
{
    options.UseSqlite("filename=WordToPdfExporter.db");
});
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

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
