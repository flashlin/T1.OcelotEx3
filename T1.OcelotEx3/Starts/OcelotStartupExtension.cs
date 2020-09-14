using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using T1.Standard.Threads;

// dotnet add package Ocelot --version 13.5.2
// dotnet add package Ocelot.Provider.Consul --version 13.5.2 
namespace T1.OcelotEx.Starts
{
    public static class OcelotStartupExtension
    {
        public static void ConfigureOcelotServices(this IServiceCollection services)
        {
            services.AddOcelot()
                .AddConsul();
        }

        public static void StartOcelot(this IApplicationBuilder app)
        {
            app.UseOcelot().Wait();
        }
    }
}
