using System.Collections.Generic;

namespace T1.OcelotEx.GrpcProtocols
{
	public class GrpcServiceDescriptor
	{
		public Dictionary<string, Dictionary<string,string>> Descriptor { get; set; }
	}
}