using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AATWebApp
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
            services.AddControllersWithViews();

            services.AddHttpClient<OAuthService>();
            services.AddHttpClient<ApiManagerService>();
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public class OAuthService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public OAuthService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }

        public async Task<string> GetDeveloperToken()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _config["oauth:endpoint"])
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"client_id", _config["oauth:client_id"]},
                    {"client_secret", _config["oauth:client_secret"]},
                    {"grant_type", _config["oauth:grant_type"]},
                    {"resource", _config["oauth:resource"]}
                }.ToArray())
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return (string)JObject.Parse(await response.Content.ReadAsStringAsync())["access_token"];
        }
    }

    public class ApiManagerService
    {
        public HttpClient Client { get; }

        public ApiManagerService(HttpClient client, OAuthService oauthService, IHostEnvironment env, IConfiguration config, ILogger<ApiManagerService> logger)
        {
            
            string token;

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", config["api:key"]);
            logger.LogCritical("api key: " + config["api:key"]);

            if (env.IsDevelopment())
            {
                // Get Developer OAuth token
                token = oauthService.GetDeveloperToken().Result;
            }
            else
            {
                // Get Managed Identity token
                var tokenProvider = new AzureServiceTokenProvider();
                token = tokenProvider.GetAccessTokenAsync(config["oauth:resource"]).Result;
            }

            logger.LogCritical(token);
            var auth = new AuthenticationHeaderValue("bearer", token);
            client.DefaultRequestHeaders.Authorization = auth;

            Client = client;
        }
    }
}
