
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddHttpClient<DeveloperService>();
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

        public class DeveloperService
        {
            private readonly HttpClient _client;
            private readonly IConfiguration _config;

            public DeveloperService(HttpClient client, IConfiguration config)
            {
                _client = client;
                _config = config;
            }

            public async Task<string> GetDeveloperToken()
            {

                var response = await _client.RequestAuthorizationCodeTokenAsync(
                    new AuthorizationCodeTokenRequest
                    {
                        Address = _config["oauth:endpoint"],
                        ClientId = _config["oauth:client_id"],
                        ClientSecret = _config["oauth:client_secret"],
                        Parameters = new Dictionary<string, string> { { "resource", _config["oauth:resource"] } }
                    });

                return response.AccessToken;
            }
        }

        public class ApiManagerService
        {
            public HttpClient Client { get; }

            public ApiManagerService(HttpClient client, DeveloperService devService, IHostEnvironment env, IConfiguration config)
            {

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", config["api:key"]);

                // If DEV environment, get DEV OAuth token,
                // else get Managed Identity token
                var token = env.IsDevelopment() ? 
                    devService.GetDeveloperToken().Result : 
                    new AzureServiceTokenProvider().GetAccessTokenAsync(config["oauth:resource"]).Result;

                var auth = new AuthenticationHeaderValue("bearer", token);
                client.DefaultRequestHeaders.Authorization = auth;

                Client = client;
            }
        }

    }
}
