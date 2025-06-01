using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TalentLink.Frontend.Models;

namespace TalentLink.Frontend.Pages
{
    public partial class ProfilPage
    {
        List<Job> createdJobs = new();
        List<MyApplicationDto> appliedJobs = new();
        bool loadingJobs = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadCreatedJobsAsync();
            await LoadAppliedJobsListAsync();
        }

        async Task LoadCreatedJobsAsync()
        {
            if (AuthService.Role != "Senior") return;
            loadingJobs = true;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7024/api/Job/created-by-me");
                if (!string.IsNullOrEmpty(AuthService.Token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthService.Token);
                }
                var response = await HttpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var jobs = await response.Content.ReadFromJsonAsync<List<Job>>();
                    if (jobs != null)
                        createdJobs = jobs;
                }
            }
            finally
            {
                loadingJobs = false;
            }
        }
        async Task LoadAppliedJobsListAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7024/api/Application/mine");
            if (!string.IsNullOrEmpty(AuthService.Token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthService.Token);
            }
            var response = await HttpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var jobs = await response.Content.ReadFromJsonAsync<List<MyApplicationDto>>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                });
                if (jobs != null)
                {
                    appliedJobs = jobs; 
                }
            }
        }
        async Task CancelApplication(Guid applicationId)
        {
            
            var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:7024/api/Application/{applicationId}");
            if (!string.IsNullOrEmpty(AuthService.Token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthService.Token);
            }
            var response = await HttpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                // Remove from local list and refresh UI
                var toRemove = appliedJobs.FirstOrDefault(a => a.ApplicationId == applicationId);
                if (toRemove != null)
                {
                    appliedJobs.Remove(toRemove);
                    StateHasChanged();
                }
            }
            
        }
        
        void createJob() => Navi.NavigateTo("/createjob");
    }
}
