using System.Runtime.Serialization;

namespace BlazorWasmGrpcApi.Shared.Models;

[DataContract]
public class IncreaseRequest
{
	[DataMember(Order = 1)]
	public int ByAmount { get; set; }
}

[DataContract]
public class IncreaseResult
{
	[DataMember(Order = 1)]
	public int NewValue { get; set; }
}