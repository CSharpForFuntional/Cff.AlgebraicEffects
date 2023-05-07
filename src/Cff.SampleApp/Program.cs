using Cff.SampleApp.Dto;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/echo", EchoAsync) 
   .Accepts<EchoDto>("application/json")
   .WithName("Echo")
   .WithOpenApi();

await app.RunAsync();

