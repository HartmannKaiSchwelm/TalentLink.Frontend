using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TalentLink.Frontend.Models;

namespace TalentLink.Frontend.Pages
{
    public partial class RegisterPage
    {
        private UserRegisterDto user = new UserRegisterDto();
        private string message;
        private string errorMessage;
        [Inject] private NavigationManager Navi { get; set; } = default!; 

        private async Task Register()
        {
            var response = await HttpClient.PostAsJsonAsync("https://talentlink-9aef45cf7016.herokuapp.com/api/Auth/register", user);

            if (response.IsSuccessStatusCode)
            {
                message = "User created successfully.";
                user = new UserRegisterDto();
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

            //go-to-login
            if (response.IsSuccessStatusCode)
            {
                Navi.NavigateTo("/login");
            }
        }

       
    }
}
