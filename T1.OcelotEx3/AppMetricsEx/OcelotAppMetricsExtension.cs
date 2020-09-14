using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace T1.OcelotEx.AppMetricsEx
{
    public static class OcelotAppMetricsExtension
    {
        public static void AddOcelotAppMetrics(this IServiceCollection services)
        {
            services.ConfigureOptions<OcelotMetricsConfig>();

            using (var sp = services.BuildServiceProvider())
            {
                var ocelotMetricsConfig = sp.GetRequiredService<IOptions<OcelotMetricsConfig>>().Value;
                var app = new AppMetricsBuilder();
                var metrics = app.Build(ocelotMetricsConfig);
                services.AddMetrics(metrics);
            }

            services.AddMetricsReportingHostedService();
        }
    }
}