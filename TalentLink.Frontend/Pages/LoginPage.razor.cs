using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TalentLink.Frontend.Models;
using TalentLink.Frontend.Services;

namespace TalentLink.Frontend.Pages
{
    public partial class LoginPage
    {
        private LoginDto login = new LoginDto();
        private string? message;

        private async Task LoginUser()
        {
            var response = await HttpClient.PostAsJsonAsync("https://talentlink-9aef45cf7016.herokuapp.com/api/auth/login", login);
            if (response.IsSuccessStatusCode)
            {
                var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
                if (auth is not null)
                {
                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);

                    await AuthService.SetAuthAsync(auth.Token, auth.Name, auth.Role, auth.Email, auth.CreatedJobs, auth.VerifiedByParentId, auth.Id, auth.ZipCode, auth.City);
                }
                if (AuthService.Role == "Admin")
                {
                    Navi.NavigateTo("/admindashboard");
                }
                else
                {
                    Navi.NavigateTo("/profile");
                }

            }
            else
            {
                message = "Login fehlgeschlagen.";
            }
        }
    }
}
