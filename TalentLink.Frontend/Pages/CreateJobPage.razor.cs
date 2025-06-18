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
                // Debug: Zeige die rohe Antwort der Categories API
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7024/api/categories");
                if (!string.IsNullOrEmpty(AuthService.Token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthService.Token);
                }

                var response = await HttpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var rawContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Categories API Response: {rawContent}");

                    var result = await response.Content.ReadFromJsonAsync<List<JobCategory>>();
                    if (result != null)
                        categories = result;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    errorMessage = $"Categories API Fehler ({response.StatusCode}): {errorContent}";
                    Console.WriteLine($"Categories API Error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Kategorien konnten nicht geladen werden: {ex.Message}";
                Console.WriteLine($"Categories loading error: {ex}");
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

            // Setze CreatedById, falls vorhanden
            try
            {
                var userId = AuthService.GetCurrentUserId();
                Console.WriteLine($"AuthService.GetCurrentUserId() returned: {userId}");

                if (!userId.HasValue || userId.Value == Guid.Empty)
                {
                    errorMessage = "Fehler: Deine UserId konnte nicht ermittelt werden. Bitte lade die Seite neu oder logge dich erneut ein.";
                    StateHasChanged();
                    return;
                }
                newJob.CreatedById = userId.Value; // Verwende userId statt AuthService.UserId
                Console.WriteLine($"Set CreatedById to: {newJob.CreatedById}");
            }
            catch (Exception ex)
            {
                errorMessage = $"Fehler beim Ermitteln der User-ID: {ex.Message}";
                Console.WriteLine($"AuthService error: {ex}");
                StateHasChanged();
                return;
            }

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
                        // Stripe-Session anfordern - mit besserer Fehlerbehandlung
                        try
                        {
                            var stripeRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7024/api/payment/create-session")
                            {
                                Content = JsonContent.Create(new
                                {
                                    jobId = createdJob.Id,
                                    amount = 499,
                                    successUrl = $"{Navi.BaseUri}payment/success",
                                    cancelUrl = $"{Navi.BaseUri}payment/cancel"
                                })
                            };

                            if (!string.IsNullOrEmpty(AuthService.Token))
                            {
                                stripeRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthService.Token);
                            }

                            var stripeResponse = await HttpClient.SendAsync(stripeRequest);

                            if (stripeResponse.IsSuccessStatusCode)
                            {
                                // Debug: Zeige die rohe Antwort
                                var rawContent = await stripeResponse.Content.ReadAsStringAsync();
                                Console.WriteLine($"Stripe API Response: {rawContent}");

                                try
                                {
                                    var stripeSession = await stripeResponse.Content.ReadFromJsonAsync<StripeSessionResponse>();
                                    if (stripeSession != null && !string.IsNullOrEmpty(stripeSession.Url))
                                    {
                                        Navi.NavigateTo(stripeSession.Url, forceLoad: true);
                                        return;
                                    }
                                    else
                                    {
                                        errorMessage = "Stripe-Session URL ist leer oder ungültig.";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errorMessage = $"Fehler beim Parsen der Stripe-Antwort: {ex.Message}";
                                    Console.WriteLine($"Stripe parsing error: {ex}");
                                }
                            }
                            else
                            {
                                var errorContent = await stripeResponse.Content.ReadAsStringAsync();
                                errorMessage = $"Stripe-Fehler ({stripeResponse.StatusCode}): {errorContent}";
                                Console.WriteLine($"Stripe API Error: {stripeResponse.StatusCode} - {errorContent}");
                            }
                        }
                        catch (Exception ex)
                        {
                            errorMessage = $"Stripe-Verbindungsfehler: {ex.Message}";
                            Console.WriteLine($"Stripe connection error: {ex}");
                        }
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    errorMessage = "Nicht autorisiert. Bitte einloggen.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    errorMessage = $"Fehler beim Speichern des Jobs: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Serverfehler beim Speichern des Jobs: {ex.Message}";
                Console.WriteLine($"Job creation error: {ex}");
            }

            StateHasChanged();
            await Task.Delay(2000);
            successMessage = null;
            errorMessage = null;
            StateHasChanged();
        }

        private class StripeSessionResponse
        {
            public string Url { get; set; } = string.Empty;
        }
    }
}