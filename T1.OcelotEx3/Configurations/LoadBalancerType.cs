namespace T1.OcelotEx.Configurations
{
	public enum LoadBalancerType
	{
		RoundRobin,
		LeastConnection,
		CookieStickySessions,
		NoLoadBalancer
	}
}