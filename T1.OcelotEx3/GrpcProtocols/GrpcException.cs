using System;

namespace T1.OcelotEx.GrpcProtocols
{
	public class GrpcException : Exception
	{
		public int StatusCode { get; set; }
	}
}