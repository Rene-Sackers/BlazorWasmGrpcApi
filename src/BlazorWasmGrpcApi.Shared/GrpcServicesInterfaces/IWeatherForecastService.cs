using System.ServiceModel;
using BlazorWasmGrpcApi.Shared.Models;

namespace BlazorWasmGrpcApi.Shared.GrpcServicesInterfaces;

[ServiceContract]
[GrpcService]
public interface IWeatherForecastService
{
	Task<WeatherForecast[]> GetForecastForLocation(string location);
	Task<IncreaseResult> Increase(IncreaseRequest increaseRequest);
}