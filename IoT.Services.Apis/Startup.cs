using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IoT.Services.EventBus;
using IoT.Services.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using IoT.Services.Contracts.Eventing;
using IoT.Services.Contracts.Messaging;
using IoT.Services.SignalRWebService.Eventing;
using Microsoft.AspNetCore.SignalR;

namespace IoT.Services.SignalRWebService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc().AddControllersAsServices();
            services.AddSingleton<IEventBus, EventBusService>(sp => { return EventBusFactory.GetEventBus(); });
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("MyPolicy"));
            });
            services.AddSignalR();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("MyPolicy");
            app.UseMvc();
            app.UseSignalR(routes =>
            {
                routes.MapHub<SignalRService>("NotifierHub");
            });

            var hub = app.ApplicationServices.GetRequiredService<IHubContext<SignalRService>>();
            //var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();            
            //var eventHandler = new MessageReceivedHandler();


        }
    }
}
