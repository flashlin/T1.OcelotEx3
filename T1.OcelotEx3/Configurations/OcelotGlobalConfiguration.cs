using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace T1.OcelotEx.Configurations
{
	public class GlobalConfiguration
	{
		public string BaseUrl { get; set; }
		public string RequestIdKey { get; set; }

		[JsonConverter(typeof(StringEnumConverter), false)]
		public DownstreamSchemeType DownstreamScheme { get; set; }
		public ServiceDiscoveryProviderConfiguration ServiceDiscoveryProvider { get; set; }
		public RateLimitOptionConfiguration RateLimitOptions { get; set; }
		public LoadBalancerOptionsConfiguration LoadBalancerOptions { get; set; }
	}
}
