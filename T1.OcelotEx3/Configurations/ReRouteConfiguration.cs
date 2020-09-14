namespace T1.OcelotEx.Configurations
{
	public class ReRouteConfiguration
	{
		public string UpstreamPathTemplate { get; set; }
		public string[] UpstreamHttpMethod { get; set; }
		public string ServiceName { get; set; }
		public string DownstreamPathTemplate { get; set; }
		public string DownstreamScheme { get; set; }
		public ReRouteDownstreamConfiguration[] DownstreamHostAndPorts { get; set; } = new ReRouteDownstreamConfiguration[0];
	}
}