using FileConvertTask;
using Filelogicdata;
using FileReaderData;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Kumaresan");
builder.Services.AddDbContextPool<FileDbContext>(option => option.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddScoped<IFilelogicclass, Filelogicclass>();
builder.Services.AddScoped<IFileReaderClass, FileReaderClass>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
