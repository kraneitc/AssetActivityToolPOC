using AATShared;
using AATWebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AATWebApi
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
            services.AddControllers();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            //    {
            //        options.Audience = "a95bcc88-192e-45d0-a4c3-f211c8ad550d";
            //        options.Authority = "https://login.microsoftonline.com/90f5a437-d7ed-4905-ad39-adc1f0d6b579";
            //        options.Configuration = new OpenIdConnectConfiguration
            //        {
            //            AuthorizationEndpoint = "https://login.microsoftonline.com/90f5a437-d7ed-4905-ad39-adc1f0d6b579/.well-known/openid-configuration",
            //            Issuer = "https://sts.windows.net/90f5a437-d7ed-4905-ad39-adc1f0d6b579/"
            //        };
            //    });

            services.AddSingleton<QueueService>();
            services.AddDbContext<AATDbContext>(options => 
                options.UseSqlServer(Configuration["SqlServer"]));
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();


            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            //app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
