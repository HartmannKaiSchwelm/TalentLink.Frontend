using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TalentLink.Frontend.Services;
using Blazored.LocalStorage;
using System.Text.Json;
using System.Threading.Tasks;

namespace TalentLink.Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddScoped<AuthenticationService>();
            builder.Services.AddBlazoredLocalStorage();
            
            // ApiConfig laden
            using var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            var configJson = await http.GetStringAsync("appsettings.json");
            var config = JsonSerializer.Deserialize<ApiConfig>(configJson);
            builder.Services.AddSingleton(config ?? new ApiConfig());
            
            // HttpClient mit ApiConfig konfigurieren
            builder.Services.AddScoped(sp => 
            {
                var apiConfig = sp.GetRequiredService<ApiConfig>();
                return new HttpClient { BaseAddress = new Uri(apiConfig.ApiBaseUrl) };
            });

            var host = builder.Build();
            var authService = host.Services.GetRequiredService<AuthenticationService>();
            await authService.TryRestoreAuthAsync();
            await host.RunAsync();
        }
    }
}
