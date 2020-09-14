using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace T1.OcelotEx.GrpcProtocols
{
	public class GrpcHttpContent : HttpContent
	{
		private readonly string _result;

		public GrpcHttpContent(string result)
		{
			this._result = result;
		}

		public GrpcHttpContent(object result)
		{
			this._result = Newtonsoft.Json.JsonConvert.SerializeObject(result);
		}

		protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
		{
			var writer = new StreamWriter(stream);
			await writer.WriteAsync(_result);
			await writer.FlushAsync();
		}

		protected override bool TryComputeLength(out long length)
		{
			length = _result.Length;
			return true;
		}
	}

}
