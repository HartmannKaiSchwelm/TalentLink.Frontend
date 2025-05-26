using System.Net.Http.Json;
using TalentLink.Frontend.Models;

namespace TalentLink.Frontend.Pages
{
    public partial class AdminDashboardPage
    {
        //private AdminDashboardDto userData;
        //protected override async Task OnInitializedAsync()
        //{
        //    await LoadAdminDashboard();
        //}

        //async Task LoadAdminDashboard()
        //{
        //    try
        //    {
        //        var response = await HttpClient.GetAsync("https://localhost:7024/api/Admin/dashboard");
        //        response.EnsureSuccessStatusCode();

        //        var fetchedDashboard = await response.Content.ReadFromJsonAsync<AdminDashboardDto>();

        //        if (fetchedDashboard != null)
        //        {
        //            userData = fetchedDashboard;
        //        }
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        Console.WriteLine($"Error fetching dashboard: {ex.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Unexpected error: {ex.Message}");
        //    }
        //}
    }
}
