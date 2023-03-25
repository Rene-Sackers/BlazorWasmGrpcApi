﻿using System.Runtime.Serialization;

namespace BlazorWasmGrpcApi.Shared.Models;

[DataContract]
public class WeatherForecast
{
	[DataMember(Order = 1)]
	public DateTime Date { get; set; }
	
	[DataMember(Order = 2)]
	public string Location { get; set; }

	[DataMember(Order = 3)]
	public int TemperatureC { get; set; }

	[DataMember(Order = 4)]
	public string Summary { get; set; }
	
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}