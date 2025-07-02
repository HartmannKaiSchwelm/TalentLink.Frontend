using System.Net.Http;
using System.Net.Http.Json;
using TalentLink.Frontend.Models;
using TalentLink.Frontend.Services;
using Microsoft.AspNetCore.Components;

namespace TalentLink.Frontend.Pages
{
    public partial class AddAdminAccountPage
    {
        private UserRegisterDto user = new UserRegisterDto();

        private async Task Register()
        {
            var response = await httpclient.PostAsJsonAsync("https://localhost:7024/api/Auth/register", user);

            if (response.IsSuccessStatusCode)
            {
                user = new UserRegisterDto();
                user.Role = 3; // Set role to Admin
            }
            StateHasChanged();
        }
    }
}
