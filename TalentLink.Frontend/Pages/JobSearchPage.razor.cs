using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TalentLink.Frontend.Models;

namespace TalentLink.Frontend.Pages
{
    public partial class JobSearchPage
    {
        [Parameter]
        public Guid jobId { get; set; }
        private List<Job> jobs = new();
        private List<JobCategory> categories = new();
        private string catUID = string.Empty;
        private bool isLoading = false;
        private string errorMessage = string.Empty;
        private string notAuthenticatedMessage = string.Empty;
        private bool showNotAuthenticatedModal = false; 

        protected override async Task OnInitializedAsync()
        {
            await LoadCategories();
            await GetAllJobs();
        }

        private async Task LoadCategories()
        {
            try
            {
                isLoading = true;
                errorMessage = string.Empty;
                var response = await HttpClient.GetAsync("https://localhost:7024/api/Categories");
                if (response.IsSuccessStatusCode)
                {
                    categories = await response.Content.ReadFromJsonAsync<List<JobCategory>>() ?? new List<JobCategory>();
                    if (!categories.Any())
                    {
                        errorMessage = "No categories returned from the API.";
                    }
                }
                else
                {
                    errorMessage = $"Failed to fetch categories: {response.StatusCode} {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error fetching categories: {ex.Message}";
                categories = new List<JobCategory>();
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private async Task GetAllJobs()
        {
            try
            {
                isLoading = true;
                errorMessage = string.Empty;
                var response = await HttpClient.GetAsync("https://localhost:7024/api/Job");
                if (response.IsSuccessStatusCode)
                {
                    var allJobs = await response.Content.ReadFromJsonAsync<List<Job>>() ?? new List<Job>();
                    // Boosted zuerst, dann nach CreatedAt, nur bezahlte Jobs
                    jobs = allJobs.Where(j => j.IsPaid)
                        .OrderByDescending(j => j.IsBoosted)
                        .ThenByDescending(j => j.CreatedAt)
                        .ToList();
                    if (!jobs.Any())
                    {
                        errorMessage = "No jobs returned from the API.";
                    }
                }
                else
                {
                    errorMessage = $"Failed to fetch jobs: {response.StatusCode} {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error fetching jobs: {ex.Message}";
                jobs = new List<Job>();
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private async Task OnCategoryChanged(ChangeEventArgs e)
        {
            catUID = e.Value?.ToString() ?? string.Empty;
            await GetJobsByCat();
        }

        private async Task GetJobsByCat()
        {
            try
            {
                isLoading = true;
                errorMessage = string.Empty;
                var url = string.IsNullOrEmpty(catUID)
                    ? "https://localhost:7024/api/Job"
                    : $"https://localhost:7024/api/Job?categoryId={catUID}";
                var response = await HttpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var allJobs = await response.Content.ReadFromJsonAsync<List<Job>>() ?? new List<Job>();
                    // Boosted zuerst, dann nach CreatedAt, nur bezahlte Jobs
                    jobs = allJobs.Where(j => j.IsPaid)
                        .OrderByDescending(j => j.IsBoosted)
                        .ThenByDescending(j => j.CreatedAt)
                        .ToList();
                    if (!jobs.Any())
                    {
                        errorMessage = "No jobs found for the selected category.";
                    }
                }
                else
                {
                    errorMessage = $"Failed to fetch jobs: {response.StatusCode} {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error fetching jobs: {ex.Message}";
                jobs = new List<Job>();
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
        
        public void ApplyTo(Guid jobId)
        {
            if (AuthService.VerifiedByParentId != null)
            {
                Navi.NavigateTo($"/apply/{jobId}");
            }
            else
            {
                notAuthenticatedMessage = "Deine Eltern haben dich nicht für die Bewerbung freigeschaltet. Bitte kontaktiere sie, um deine Bewerbung zu starten";
                showNotAuthenticatedModal = true; 
            }
        }
    }
}