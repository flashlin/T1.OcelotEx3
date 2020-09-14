using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Request.Mapper;
using Ocelot.Responses;

namespace T1.OcelotEx.GrpcProtocols
{
	public class GrpcRequestBuilder
	{
		private readonly IOcelotLogger logger;
		private readonly GrpcServiceDescriptor descriptor;

		public GrpcRequestBuilder(IOcelotLoggerFactory factory,
			GrpcServiceDescriptor descriptor)
		{
			this.logger = factory.CreateLogger<GrpcRequestBuilder>();
			this.descriptor = descriptor;
		}

		public Response<GrpcRequest> BuildRequest(HttpContext context)
		{
			var route = context.Request.Path.Value.Trim('/').Split('/');
			if (route.Length != 2)
			{
				return SetError($"error request:{route},must do like this:http://domain:port/grpc/ServiceName/MethordName/");
			}
			string svcName = route[0].ToUpper();
			string methodName = route[1].ToUpper();

			var grpcDescript = descriptor.Descriptor;
			if (!grpcDescript.ContainsKey(svcName))
			{
				return SetError($"service name is not defined.{svcName}");
			}

			if (!grpcDescript[svcName].ContainsKey(methodName))
			{
				return SetError($"method name is not defined.{methodName}");
			}

			var grpcRequest = new GrpcRequest
			{
				GrpcMethod = grpcDescript[svcName][methodName],
				Headers = GetRequestHeaders(context)
			};

			try
			{
				//需要替换Scheme
				context.Request.Scheme = "http";
				var requestJson = GetRequestJson(context);
				//grpcRequest.RequestMessage = JsonConvert.DeserializeObject(requestJson, grpcRequest.GrpcMethod.InputType.ClrType);
			}
			catch (Exception)
			{
				return SetError("request parameter error");
			}
			context.Request.Scheme = "grpc";
			return new OkResponse<GrpcRequest>(grpcRequest);
		}

		private string GetRequestJson(HttpContext context)
		{
			if (context.Request.Method == "GET")
			{
				JObject o = new JObject();
				foreach (var q in context.Request.Query)
				{
					o.Add(q.Key, q.Value.ToString());
				}
				return JsonConvert.SerializeObject(o);
			}
			else
			{
				var json = "{}";
				var encoding = context.Request.GetTypedHeaders().ContentType?.Encoding ?? Encoding.UTF8;
				using (var sr = new StreamReader(context.Request.Body, encoding))
				{
					json = sr.ReadToEnd();
				}
				return json;
				//var requestMessage = context.DownstreamRequest.ToHttpRequestMessage();
				//var stream = requestMessage.Content.ReadAsStreamAsync().Result;
				//using (var reader = new StreamReader(stream, encoding))
				//{
				//    return reader.ReadToEnd();
				//}
			}
		}

		// http header to grpc header
		private IDictionary<string, string> GetRequestHeaders(HttpContext context)
		{
			Dictionary<string, string> headers = new Dictionary<string, string>();
			foreach (string key in context.Request.Headers.Keys)
			{
				string grpcKey = null;
				string prefix = "grpc.";
				if (key.Length > prefix.Length && key.StartsWith(prefix))
				{
					grpcKey = key.Substring(prefix.Length);
				}
				else
				{
					continue;
				}
				Microsoft.Extensions.Primitives.StringValues value = context.Request.Headers[key];
				headers.Add(grpcKey, value.FirstOrDefault());
			}
			return headers;
		}

		private ErrorResponse<GrpcRequest> SetError(Exception exception)
		{
			return new ErrorResponse<GrpcRequest>(new UnmappableRequestError(exception));
		}

		private ErrorResponse<GrpcRequest> SetError(string message)
		{
			var exception = new Exception(message);
			return SetError(exception);
		}
	}
}
