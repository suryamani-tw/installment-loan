var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient(); // <-- Add this line
builder.Services.AddHttpClient("DisbursementService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5062"); // DisbursementService port
});

var app = builder.Build();

app.MapControllers();
app.Run();