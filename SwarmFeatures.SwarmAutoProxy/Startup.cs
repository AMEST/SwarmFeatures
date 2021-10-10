using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwarmFeatures.SwarmAutoProxy.Configuration;
using SwarmFeatures.SwarmAutoProxy.Middlewares.Proxy;
using SwarmFeatures.SwarmControl.Extensions;
using System.Net.Http;
using SwarmFeatures.SwarmAutoProxy.Extensions;

namespace SwarmFeatures.SwarmAutoProxy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = new SwarmAutoProxyConfiguration(Configuration);
            services.AddWebSockets(opt => opt.AllowedOrigins.Add("*"));
            services.AddSwarmControlModule(configuration);
            services.AddAutoProxyModule();
            services.AddProxy(options =>
            {
                options.MessageHandler = new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => true
                };
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseWebSockets();
            app.RunProxy();
            app.UseRouting();
            app.UseEndpoints( endpoints =>{
                endpoints.MapControllers();
            });
            app.Run(async (context) => { await context.Response.WriteAsync("404 - Proxy Not Found"); });
        }
    }
}