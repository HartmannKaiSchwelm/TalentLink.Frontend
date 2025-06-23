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
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            // Enum in string
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
                DefaultRequestHeaders = { /* ... */ }
            });

            // ApiConfig laden
            using var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            var configJson = await http.GetStringAsync("appsettings.json");
            var config = JsonSerializer.Deserialize<ApiConfig>(configJson);
            builder.Services.AddSingleton(config ?? new ApiConfig());

            var host = builder.Build();
            var authService = host.Services.GetRequiredService<AuthenticationService>();
            await authService.TryRestoreAuthAsync();
            await host.RunAsync();
        }
    }
}
