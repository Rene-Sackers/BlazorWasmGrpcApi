using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorWasmGrpcApi.BlazorWasm;
using BlazorWasmGrpcApi.BlazorWasm.Extensions;
using BlazorWasmGrpcApi.Shared.GrpcServicesInterfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = builder.Configuration["ApiUrl"];
Console.WriteLine("API URL: " + apiUrl);
if (string.IsNullOrWhiteSpace(apiUrl) || !Uri.TryCreate(apiUrl, UriKind.Absolute, out _))
	throw new("Empty or invalid API URL");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Manual registration
// builder.Services.AddGrpcChannel(apiUrl);
// builder.Services.AddGrpcService<IWeatherForecastService>();

// Automatic registration
builder.AddGrpc(apiUrl);

await builder.Build().RunAsync();