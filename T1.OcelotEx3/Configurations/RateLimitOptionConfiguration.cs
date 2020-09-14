namespace T1.OcelotEx.Configurations
{
	public class RateLimitOptionConfiguration
	{
		public bool DisableRateLimitHeaders { get; set; }
		public string QuotaExceededMessage { get; set; }
		public int HttpStatusCode { get; set; }
		public string ClientIdHeader { get; set; }
	}
}