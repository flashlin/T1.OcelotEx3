using System;
using System.Collections.Generic;
using System.Text;

namespace T1.OcelotEx.Configurations
{
	public class ConsulConfig
	{
		public Uri ServiceDiscoveryAddress { get; set; }
		public Uri ServiceAddress { get; set; }
		public string ServiceName { get; set; }
		public string ServiceId { get; set; }
		public string HealthCheckUrl { get; set; }
		public TimeSpan HealthCheckInterval { get; set; } = TimeSpan.FromSeconds(10);
	}
}
