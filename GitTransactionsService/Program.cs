var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "GitTransactionsService is running!");
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();