using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TalentLink.Frontend.Models;

namespace TalentLink.Frontend.Pages
{
    public partial class ApplicationPage
    {
        
        
        [Parameter]
        public Guid jobId { get; set; }

        private Job? job;
        private JobApplication application = new();
        private string? successMessage;
        private string? errorMessage;

        protected override async Task OnInitializedAsync()
        {
            // Lade Jobdetails anhand der jobId
            try
            {
                job = await Httpclient.GetFromJsonAsync<Job>($"https://talentlink-9aef45cf7016.herokuapp.com//api/Job/{jobId}");
                application.JobId = jobId;
            }
            catch
            {
                errorMessage = "Job konnte nicht geladen werden.";
            }
        }

        private async Task SubmitApplication()
        {
            try
            {
                if (!string.IsNullOrEmpty(AuthService.Token))
                {
                    Httpclient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthService.Token);
                }
                var response = await Httpclient.PostAsJsonAsync($"https://talentlink-9aef45cf7016.herokuapp.com//api/Job/{jobId}/apply", application);
                if (response.IsSuccessStatusCode)
                {
                    successMessage = "Bewerbung erfolgreich abgeschickt!";
                }
                else
                {
                    errorMessage = "Fehler beim Absenden der Bewerbung.";
                }
            }
            catch
            {
                errorMessage = "Serverfehler beim Absenden der Bewerbung.";
            }
            StateHasChanged();
            await Task.Delay(2000);
            successMessage = null;
            errorMessage = null;
            StateHasChanged();
        }
    }
}
