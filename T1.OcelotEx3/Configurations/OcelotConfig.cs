using Newtonsoft.Json;

namespace T1.OcelotEx.Configurations
{
	public class OcelotConfig
	{
		public ReRouteConfiguration[] ReRoutes { get; set; } = new ReRouteConfiguration[0];
		public AggregateConfiguration[] Aggregates { get; set; } = new AggregateConfiguration[0];
		public GlobalConfiguration GlobalConfiguration { get; set; } = new GlobalConfiguration();

		public string ToJson()
		{
			return JsonConvert.SerializeObject(
				this,
				Formatting.None);
		}
	}
}