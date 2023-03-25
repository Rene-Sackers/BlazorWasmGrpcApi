using System.IO.Compression;
using BlazorWasmGrpcApi.Api.Extensions;
using BlazorWasmGrpcApi.Api.GrpcServices;
using BlazorWasmGrpcApi.Shared.GrpcServicesInterfaces;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCodeFirstGrpc(config =>
{
	config.ResponseCompressionLevel = CompressionLevel.Optimal;
});

builder.Services.AddCors(o =>
{
	o.AddDefaultPolicy(corsPolicyBuilder =>
	{
		corsPolicyBuilder
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

// Manual registration - Doesn't work atm for some reason. God knows why.
// app.UseGrpcWeb(new() { DefaultEnabled = true });
// app.MapGrpcService<IWeatherForecastService>();

// Automatic registration
app.MapGrpcServices();

app.UseCors();

app.Run();