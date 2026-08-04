using GitTransactionsService.Data;
using GitTransactionsService.Interfaces;
using GitTransactionsService.Services;
using Microsoft.EntityFrameworkCore;
using ProjeninAdi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITransactionRepository, EfTransactionRepository>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=payment_system.db"));

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.MapGet("/", () => "GitTransactionsService is running!");
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();