using System;
using App.Metrics;

namespace T1.OcelotEx.AppMetricsEx
{
	public class AppMetricsBuilder
	{
		public IMetricsRoot Build(OcelotMetricsConfig ocelotMetricsConfig)
		{
			var metrics = AppMetrics.CreateDefaultBuilder()
				.Configuration
				.Configure(options =>
				{
					options.AddAppTag(ocelotMetricsConfig.App);
					options.AddEnvTag(ocelotMetricsConfig.Env);
				})
				.Report
				.ToInfluxDb(options =>
				{
					var influxDb = options.InfluxDb;
					influxDb.BaseUri = new Uri(ocelotMetricsConfig.ConnectionString);
					influxDb.Database = ocelotMetricsConfig.DatabaseName;
					influxDb.UserName = ocelotMetricsConfig.UserName;
					influxDb.Password = ocelotMetricsConfig.Password;
					options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
					options.HttpPolicy.FailuresBeforeBackoff = 5;
					options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
					options.FlushInterval = TimeSpan.FromSeconds(5);
				})
				.Build();
			return metrics;
		}
	}
}
