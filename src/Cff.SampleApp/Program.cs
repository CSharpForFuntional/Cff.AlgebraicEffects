using Cff.SampleApp.Dto;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/echo", EchoAsync).Accepts<EchoDto>("application/json").WithName("Echo").WithOpenApi();

await app.RunAsync();

