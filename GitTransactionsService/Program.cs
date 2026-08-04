using GitTransactionsService.Interfaces;
using GitTransactionsService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.MapGet("/", () => "GitTransactionsService is running!");
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();