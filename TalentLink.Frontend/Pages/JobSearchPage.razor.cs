using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Net.Http.Json;
using TalentLink.Frontend.Models;

namespace TalentLink.Frontend.Pages
{
    public partial class JobSearchPage
    {
        [Inject] private ApiConfig ApiConfig { get; set; } = default!;
        [Parameter]
        public Guid jobId { get; set; }
        private List<JobListItemDto> jobs = new();
        private List<JobCategory> categories = new();
        private string catUID = string.Empty;
        private bool isLoading = false;
        private string errorMessage = string.Empty;
        private string notAuthenticatedMessage = string.Empty;
        private bool showNotAuthenticatedModal = false;

        //Search-props
        private string searchLocation = "";
        private int searchRadius = 5;

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
                var response = await HttpClient.GetAsync("https://talentlink-9aef45cf7016.herokuapp.com/api/Categories");
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
                var response = await HttpClient.GetAsync("https://talentlink-9aef45cf7016.herokuapp.com/api/Job");
                if (response.IsSuccessStatusCode)
                {
                    var allJobs = await response.Content.ReadFromJsonAsync<List<JobListItemDto>>() ?? new List<JobListItemDto>();
                    // Boosted zuerst, dann nach CreatedAt, nur bezahlte Jobs
                    jobs = allJobs.Where(j => j.IsPaid && !j.IsAssigned)
                        .OrderByDescending(j => j.IsBoosted)
                        .ThenByDescending(j => j.CreatedAt)
                        .ToList();
                    if (!jobs.Any())
                    {
                        errorMessage = "Keine Jobs gefunden";
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
                jobs = new List<JobListItemDto>();
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
                    ? "https://talentlink-9aef45cf7016.herokuapp.com/api/Job"
                    : $"https://talentlink-9aef45cf7016.herokuapp.com/api/Job?categoryId={catUID}";
                var response = await HttpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var allJobs = await response.Content.ReadFromJsonAsync<List<JobListItemDto>>() ?? new List<JobListItemDto>();
                    // Boosted zuerst, dann nach CreatedAt, nur bezahlte Jobs und nicht vergebene
                    jobs = allJobs.Where(j => j.IsPaid && !j.IsAssigned)
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
                jobs = new List<JobListItemDto>();
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
        public async Task<(double lat, double lng)?> GetCoordinatesAsync(string ort)
        {
            var response = await HttpClient.GetAsync($"https://talentlink-9aef45cf7016.herokuapp.com/api/job/geocode?query={Uri.EscapeDataString(ort)}");
            if (!response.IsSuccessStatusCode) return null;
            var coords = await response.Content.ReadFromJsonAsync<GeoResult>();
            return (coords.Lat, coords.Lng);
        }
        private async Task SearchJobsByLocation()
        {
            var coords = await GetCoordinatesAsync(searchLocation);
            if (coords == null)
            {
                errorMessage = "Ort konnte nicht gefunden werden.";
                return; 
            }
            var (lat, lng) = coords.Value;
            var url = $"https://talentlink-9aef45cf7016.herokuapp.com/api/job/search?latitude={lat.ToString(CultureInfo.InvariantCulture)}&longitude={lng.ToString(CultureInfo.InvariantCulture)}&radiusKm={searchRadius}";
            jobs = await HttpClient.GetFromJsonAsync<List<JobListItemDto>>(url) ?? new();
        }
    
    }
}