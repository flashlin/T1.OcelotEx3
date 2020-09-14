using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Responses;

namespace T1.OcelotEx.GrpcProtocols
{
	public class OcelotGrpcHttpMiddleware : OcelotMiddleware
	{
		private readonly RequestDelegate next;
		private readonly GrpcPool grpcPool;
		private readonly GrpcRequestBuilder grpcRequestBuilder;

		public OcelotGrpcHttpMiddleware(RequestDelegate next,
			 GrpcPool grpcPool,
			 GrpcRequestBuilder grpcRequestBuilder,
			 IOcelotLoggerFactory factory) : base(factory.CreateLogger<OcelotGrpcHttpMiddleware>())
		{
			this.next = next;
			this.grpcPool = grpcPool;
			this.grpcRequestBuilder = grpcRequestBuilder;
		}

		public async Task Invoke(HttpContext context)
		{
			object result = null;
			var errMessage = string.Empty;
			var httpStatusCode = HttpStatusCode.OK;
			var buildRequest = grpcRequestBuilder.BuildRequest(context);
			if (buildRequest.IsError)
			{
				errMessage = "bad request";
				httpStatusCode = HttpStatusCode.BadRequest;
				Logger.LogWarning(errMessage);
			}
			else
			{
				try
				{
					var channel = grpcPool.GetChannel(new GrpcServiceEndpoint(context.Request.Host.Host, context.Request.Host.Port ?? 80));
					var client = new GrpcMethodDescriptorClient(channel);
					result = await client.InvokeAsync(buildRequest.Data.GrpcMethod, buildRequest.Data.Headers, buildRequest.Data.RequestMessage);
				}
				catch (GrpcException ex)
				{
					httpStatusCode = HttpStatusCode.InternalServerError;
					errMessage = $"rpc exception.";
					Logger.LogError($"{ex.StatusCode}--{ex.Message}", ex);
				}
				catch (Exception ex)
				{
					httpStatusCode = HttpStatusCode.ServiceUnavailable;
					errMessage = $"error in request grpc service.";
					Logger.LogError($"{errMessage}--{context.Request.Path.Value}", ex);
				}
			}
			OkResponse<GrpcHttpContent> httpResponse;
			if (string.IsNullOrEmpty(errMessage))
			{
				httpResponse = new OkResponse<GrpcHttpContent>(new GrpcHttpContent(result));
			}
			else
			{
				httpResponse = new OkResponse<GrpcHttpContent>(new GrpcHttpContent(errMessage));
			}
			context.Response.ContentType = "application/json";
			var downstreamResponse = new DownstreamResponse(httpResponse.Data, httpStatusCode, httpResponse.Data.Headers,
				"OcelotGrpcHttpMiddleware");
			//TODO: ???
			//context.Response.Write =;
		}
	}

	public class GrpcMethodDescriptorClient
	{
		public GrpcMethodDescriptorClient(object channel)
		{
			throw new NotImplementedException();
		}

		public Task<object> InvokeAsync(string dataGrpcMethod, IDictionary<string, string> dataHeaders, object dataRequestMessage)
		{
			throw new NotImplementedException();
		}
	}

	public class GrpcServiceEndpoint
	{
		public GrpcServiceEndpoint(string downstreamRequestHost, int downstreamRequestPort)
		{
			throw new NotImplementedException();
		}
	}
}
