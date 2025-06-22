using System.Net.Http;
using System.Net.Http.Json;
using TalentLink.Frontend.Models;
using TalentLink.Frontend.Services;

namespace TalentLink.Frontend.Pages
{
    public partial class AddAdminAccountPage
    {
        private UserRegisterDto user = new UserRegisterDto();
        private string message;
        private string errorMessage;

        private async Task Register()
        {
            var response = await httpclient.PostAsJsonAsync("https://localhost:7024/api/Auth/register", user);

            if (response.IsSuccessStatusCode)
            {
                message = "User created successfully.";
                user = new UserRegisterDto();
                user.Role = 3; // Set role to Admin
            }
            else
            {
                errorMessage = "Error creating user.";
            }
            StateHasChanged();
            await Task.Delay(2000);
            message = null;
            errorMessage = null;
            StateHasChanged();
        }
    }
}
