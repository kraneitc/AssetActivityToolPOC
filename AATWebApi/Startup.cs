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

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidIssuer = "https://sts.windows.net/90f5a437-d7ed-4905-ad39-adc1f0d6b579/",
            //            ValidateAudience = true,
            //            ValidAudience = "a95bcc88-192e-45d0-a4c3-f211c8ad550d",
            //            ValidateLifetime = true
            //        };
            //    });

            IdentityModelEventSource.ShowPII = true;

            //var configManager = new ConfigurationManager<OpenIdConnectConfiguration>("https://login.microsoftonline.com/90f5a437-d7ed-4905-ad39-adc1f0d6b579/v2.0/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever());
            //var openidconfig = configManager.GetConfigurationAsync().Result;

            var openidconfig = OpenIdConnectConfigurationRetriever.GetAsync("https://login.microsoftonline.com/90f5a437-d7ed-4905-ad39-adc1f0d6b579/v2.0/.well-known/openid-configuration", CancellationToken.None).Result;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.Authority = "https://login.microsoftonline.com/90f5a437-d7ed-4905-ad39-adc1f0d6b579/";
                    //x.RequireHttpsMetadata = false;
                    //x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = openidconfig.Issuer,
                        ValidAudience = "a95bcc88-192e-45d0-a4c3-f211c8ad550d",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKeys = openidconfig.SigningKeys

                    };
                    //x.Events = new JwtBearerEvents
                    //{
                    //    OnChallenge = context =>
                    //    {
                    //        return Task.CompletedTask;
                    //    },
                    //    OnMessageReceived = context =>
                    //    {
                    //        return Task.CompletedTask;
                    //    },
                    //    OnTokenValidated = context =>
                    //    {
                    //        return Task.CompletedTask;
                    //    },
                    //    OnForbidden = context =>
                    //    {
                    //        return Task.CompletedTask;
                    //    },
                    //    OnAuthenticationFailed = context =>
                    //    {
                    //        return Task.CompletedTask;
                    //    }
                    //};
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
            //app.UseIdentityServer();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
