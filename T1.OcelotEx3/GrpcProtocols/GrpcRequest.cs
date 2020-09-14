using System;
using System.Collections.Generic;
using System.Text;

namespace T1.OcelotEx.GrpcProtocols
{
	public class GrpcRequest
	{
		//public MethodDescriptor GrpcMethod { get; set; }
		public string GrpcMethod { get; set; }

		public IDictionary<string, string> Headers { get; set; }

		public object RequestMessage { get; set; }
	}
}
