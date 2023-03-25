using System.Reflection;
using BlazorWasmGrpcApi.Shared;

namespace BlazorWasmGrpcApi.Api.Extensions;

public static class ConfigureGrpcServicesExtension
{
	private static readonly Type GrpcServiceAttributeType = typeof(GrpcServiceAttribute);
	private static readonly Lazy<MethodInfo> MapGrpcServiceMethod = new(GetMapGrpcServiceMethod);

	private static MethodInfo GetMapGrpcServiceMethod()
		=> typeof(GrpcEndpointRouteBuilderExtensions).GetMethod("MapGrpcService")!;

	public static void MapGrpcServices(this WebApplication app)
	{
		var classesWithGrpcServiceAttribute = Assembly.GetExecutingAssembly()
			.GetTypes()
			.Where(t => t.IsClass && t.GetInterfaces().Any(i => i.GetCustomAttribute(GrpcServiceAttributeType) != null))
			.ToList();
		
		app.UseGrpcWeb(new() { DefaultEnabled = true });

		foreach (var type in classesWithGrpcServiceAttribute)
			app.MapGrpcService(type);
	}

	private static void MapGrpcService(this IEndpointRouteBuilder builder, Type grpcServiceType)
	{
		var genericReference = MapGrpcServiceMethod.Value.MakeGenericMethod(grpcServiceType);
		genericReference.Invoke(null, new object[] { builder });
	}
}