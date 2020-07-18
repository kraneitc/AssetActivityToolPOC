using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using AATShared;
using AATWebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

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

            IdentityModelEventSource.ShowPII = true;

            //var openidconfig = OpenIdConnectConfigurationRetriever.GetAsync("https://login.microsoftonline.com/90f5a437-d7ed-4905-ad39-adc1f0d6b579/v2.0/.well-known/openid-configuration", CancellationToken.None).Result;
            var openidconfig = OpenIdConnectConfigurationRetriever.GetAsync("https://login.microsoftonline.com/8c9b06d0-cb4a-4a03-b449-1f5a2548a910/v2.0/.well-known/openid-configuration", CancellationToken.None).Result;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://login.microsoftonline.com/8c9b06d0-cb4a-4a03-b449-1f5a2548a910/";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = openidconfig.Issuer,
                        ValidAudience = "https://sapnomsdevaatapiaeapp.nonprod-oms-ae-appsvcenv.appserviceenvironment.net",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKeys = openidconfig.SigningKeys
                    };
                });

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
