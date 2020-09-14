using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace T1.OcelotEx.Configurations
{
	public class LoadBalancerOptionsConfiguration
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public LoadBalancerType Type { get; set; }
		public string Key { get; set; }
		public int? Expiry { get; set; }
	}
}