using Microsoft.AspNetCore.Components;
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
            var response = await HttpClient.PostAsJsonAsync("https://localhost:7024/api/auth/login", login);
            if (response.IsSuccessStatusCode)
            {
                var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
                if (auth is not null)
                {
                    AuthService.SetAuthAsync(auth.Token, auth.Name, auth.Role, auth.Email, auth.CreatedJobs);
                }
                Navi.NavigateTo("/profile");
            }
            else
            {
                message = "Login fehlgeschlagen.";
            }
        }
    }
}
