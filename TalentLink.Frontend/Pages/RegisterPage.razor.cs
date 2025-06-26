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
            try
            {
                // Debug: Zeige die vollständigen User-Daten
                Console.WriteLine("=== REGISTRATION DEBUG ===");
                Console.WriteLine($"Name: '{user.Name}'");
                Console.WriteLine($"Email: '{user.Email}'");
                Console.WriteLine($"Password: '{(string.IsNullOrEmpty(user.Password) ? "EMPTY" : "SET")}'");
                Console.WriteLine($"Role: {user.Role}");
                Console.WriteLine($"JSON: {System.Text.Json.JsonSerializer.Serialize(user)}");

                var response = await HttpClient.PostAsJsonAsync("https://talentlink-9aef45cf7016.herokuapp.com/api/Auth/register", user);

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}"))}");

                if (response.IsSuccessStatusCode)
                {
                    var successContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Success Response: {successContent}");

                    message = "User created successfully.";
                    user = new UserRegisterDto();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error Response Body: {errorContent}");
                    Console.WriteLine($"Error Response Headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}"))}");

                    errorMessage = $"Error creating user: {response.StatusCode}";

                    // Versuche spezifische Fehlermeldung zu parsen
                    if (!string.IsNullOrEmpty(errorContent))
                    {
                        errorMessage += $" - {errorContent}";
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Exception: {httpEx.Message}");
                Console.WriteLine($"Stack Trace: {httpEx.StackTrace}");
                errorMessage = $"Network error: {httpEx.Message}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                errorMessage = $"Unexpected error: {ex.Message}";
            }

            StateHasChanged();

            // Clear messages nach 5 Sekunden (länger für debugging)
            await Task.Delay(5000);
            message = null;
            errorMessage = null;
            StateHasChanged();

            // Navigation nur bei Erfolg
            if (!string.IsNullOrEmpty(message) && message.Contains("successfully"))
            {
                Navi.NavigateTo("/login");
            }
        }


    }
}
