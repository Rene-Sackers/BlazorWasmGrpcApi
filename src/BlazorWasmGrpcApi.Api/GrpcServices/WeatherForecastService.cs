using BlazorWasmGrpcApi.Shared.GrpcServicesInterfaces;
using BlazorWasmGrpcApi.Shared.Models;

namespace BlazorWasmGrpcApi.Api.GrpcServices;

public class WeatherForecastService : IWeatherForecastService
{
	public Task<WeatherForecast[]> GetForecastForLocation(string location)
	{
		var random = new Random();
		var forecast = new WeatherForecast[7];
		for (var i = 0; i < forecast.Length; i++)
		{
			forecast[i] = new ()
			{
				Date = DateTime.Now.Date.AddDays(i),
				Location = location,
				Summary = "Pretty alright, honestly.",
				TemperatureC = random.Next(18, 25)
			};
		}

		return Task.FromResult(forecast);
	}

	private static int _currentValue;
	public Task<IncreaseResult> Increase(IncreaseRequest increaseRequest)
	{
		_currentValue += increaseRequest.ByAmount;
		
		return Task.FromResult(new IncreaseResult {NewValue = _currentValue});
	}
}