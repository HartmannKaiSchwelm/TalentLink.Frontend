using System.Net.Http.Json;
using TalentLink.Frontend.Models;

namespace TalentLink.Frontend.Pages
{
    public partial class CreateJobPage
    {
        private CreateJobDto newJob = new CreateJobDto();
        private List<JobCategory> categories = new();
        private Guid? selectedCategoryId;
        private string? successMessage;
        private string? errorMessage;

        protected override async Task OnInitializedAsync()
        {
            await LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                var result = await HttpClient.GetFromJsonAsync<List<JobCategory>>("https://localhost:7024/api/categories");
                if (result != null)
                    categories = result;
            }
            catch
            {
                errorMessage = "Kategorien konnten nicht geladen werden.";
            }
        }

        private async Task CreateJob()
        {
            errorMessage = null;
            successMessage = null;
            if (selectedCategoryId == null)
            {
                errorMessage = "Bitte wählen Sie eine Kategorie.";
                return;
            }
            var selectedCategory = categories.FirstOrDefault(c => c.Id == selectedCategoryId);
            if (selectedCategory == null)
            {
                errorMessage = "Ungültige Kategorie.";
                return;
            }
            newJob.Category = new JobCategory
            {
                Id = selectedCategory.Id,
                Name = selectedCategory.Name,
                ImageUrl = selectedCategory.ImageUrl
            };
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7024/api/Job")
                {
                    Content = JsonContent.Create(newJob)
                };
                if (!string.IsNullOrEmpty(AuthService.Token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthService.Token);
                }
                var response = await HttpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var createdJob = await response.Content.ReadFromJsonAsync<Job>();
                    if (createdJob != null)
                    {
                        if (AuthService.CreatedJobs == null)
                            AuthService.CreatedJobs = new List<Job>();
                        AuthService.CreatedJobs.Add(createdJob);
                    }
                    successMessage = "Job erfolgreich angelegt!";
                    newJob = new CreateJobDto();
                    selectedCategoryId = null;
                    Navi.NavigateTo("/profile", true); // true = forceLoad
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    errorMessage = "Nicht autorisiert. Bitte einloggen.";
                }
                else
                {
                    errorMessage = "Fehler beim Speichern des Jobs.";
                }
            }
            catch
            {
                errorMessage = "Serverfehler beim Speichern des Jobs.";
            }
            StateHasChanged();
            await Task.Delay(2000);
            successMessage = null;
            errorMessage = null;
            StateHasChanged();
        }
    }
}
