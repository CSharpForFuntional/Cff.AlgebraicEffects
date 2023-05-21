using Cff.SampleApp.Dto;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapPost("/echo", EchoAsync).Accepts<EchoDto>("application/json").WithName("Echo").WithOpenApi();

await app.RunAsync();

