using System.Reflection;
using BlazorWasmGrpcApi.Shared;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProtoBuf.Grpc.Client;
using ProtoBuf.Grpc.Configuration;

namespace BlazorWasmGrpcApi.BlazorWasm.Extensions;

public static class ConfigureGrpcServicesExtension
{
	private static readonly Type GrpcServiceAttributeType = typeof(GrpcServiceAttribute);
	
	private static readonly Lazy<MethodInfo> CreateGrpcServiceMethod = new(GetCreateGrpcServiceMethod);

	private static MethodInfo GetCreateGrpcServiceMethod()
		=> typeof(GrpcClientFactory).GetMethods().First(m => m.Name == "CreateGrpcService");
	
	public static void AddGrpc(this WebAssemblyHostBuilder builder, string apiUrl)
	{
		builder.Services.AddGrpcChannel(apiUrl);
		// var x = typeof(GrpcClientFactory).GetMethods().Select(m => new { m.Name, m. });
		
		var interfacesWithGrpcServiceAttribute = GrpcServiceAttributeType.Assembly
			.GetTypes()
			.Where(t => t.IsInterface && t.GetCustomAttribute(GrpcServiceAttributeType) != null)
			.ToList();
		
		foreach (var service in interfacesWithGrpcServiceAttribute)
			builder.Services.AddGrpcService(service);
	}
	
	public static void AddGrpcChannel(this IServiceCollection serviceCollection, string serverUrl)
	{
		serviceCollection.AddSingleton(_ =>
		{
			var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
			return GrpcChannel.ForAddress(serverUrl, new() { HttpHandler = httpHandler });
		});
	}

	private static void AddGrpcService(this IServiceCollection serviceCollection, Type serviceType)
	{
		Console.WriteLine("Register Grpc service: " + serviceType.FullName);
		
		serviceCollection.AddTransient(serviceType, s =>
		{
			try
			{
				var grpcChannel = s.GetRequiredService<GrpcChannel>();
				var genericReference = CreateGrpcServiceMethod.Value.MakeGenericMethod(serviceType);
				return genericReference.Invoke(null, new object[] { grpcChannel, null })!;
			}
			catch (Exception e)
			{
				Console.WriteLine($"Failed to register Grpc Service of type: {serviceType.FullName}");
				throw;
			}
		});
	}

	public static void AddGrpcService<TService>(this IServiceCollection serviceCollection) where TService : class
	{
		serviceCollection.AddTransient(s =>
		{
			try
			{
				var grpcChannel = s.GetRequiredService<GrpcChannel>();
				return grpcChannel.CreateGrpcService<TService>();
			}
			catch (Exception e)
			{
				Console.WriteLine($"Failed to register Grpc Service of type: {typeof(TService).FullName}");
				throw;
			}
		});
	}
}