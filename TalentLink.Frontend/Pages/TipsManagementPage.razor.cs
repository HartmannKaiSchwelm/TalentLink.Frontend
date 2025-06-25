using System.Net.Http.Json;
using TalentLink.Frontend.Models;

namespace TalentLink.Frontend.Pages
{
    public partial class TipsManagementPage
    {
        private List<Tip> tips = new();
        private bool showEditModal = false;
        private Tip? editTip;
        private string? successMessage;
        private string? errorMessage;
        // Create
        private bool showCreateModal = false;
        private TipCreateDto newTip = new();
        private string? successMessageCreate;
        private string? errorMessageCreate;
        private string? DeleteMessage;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await Httpclient.GetAsync("https://talentlink-9aef45cf7016.herokuapp.com/api/Tips");
                response.EnsureSuccessStatusCode();
                var fetchedTips = await response.Content.ReadFromJsonAsync<List<Tip>>();
                if (fetchedTips != null)
                {
                    tips = fetchedTips;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching tips: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        async Task EditTip(Guid id)
        {
            // nicht genutzt
        }

        async Task DeleteTip(Guid id)
        {
            try
            {
                if (!string.IsNullOrEmpty(AuthService.Token))
                {
                    Httpclient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthService.Token);
                }
                var response = await Httpclient.DeleteAsync($"https://talentlink-9aef45cf7016.herokuapp.com/api/Tips/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var tipToRemove = tips.FirstOrDefault(t => t.Id == id);
                    if (tipToRemove != null)
                    {
                        tips.Remove(tipToRemove);
                    }
                }
                else
                {
                    // Optional: Fehlerbehandlung
                }
                DeleteMessage = "Tip gelöscht";
                StateHasChanged(); 
                await Task.Delay(2000);
                DeleteMessage = null; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Löschen: {ex.Message}");
            }
        }

        //Edit
        async Task EditTip(Tip tip)
        {
            // Kopie erstellen, damit Änderungen erst beim Speichern übernommen werden
            editTip = new Tip
            {
                Id = tip.Id,
                Title = tip.Title,
                Content = tip.Content,
                CreatedAt = tip.CreatedAt,
                CreatedById = tip.CreatedById,
                CreatedBy = null // Wichtig: null setzen, damit kein komplexes Objekt gesendet wird
            };
            showEditModal = true;
            successMessage = null;
            errorMessage = null;
        }

        void CloseEditModal()
        {
            showEditModal = false;
            editTip = null;
            successMessage = null;
            errorMessage = null;
        }

        async Task SaveTip()
        {
            if (editTip == null) return;
            var updateDto = new TipUpdateDto
            {
                Title = editTip.Title,
                Content = editTip.Content
            };
           
            if (!string.IsNullOrEmpty(AuthService.Token))
            {
                Httpclient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthService.Token);
            }
            var response = await Httpclient.PutAsJsonAsync($"https://talentlink-9aef45cf7016.herokuapp.com/api/Tips/{editTip.Id}", updateDto);
            if (response.IsSuccessStatusCode)
            {
                var idx = tips.FindIndex(t => t.Id == editTip.Id);
                if (idx >= 0)
                {
                    tips[idx] = editTip;
                }
                successMessage = "Erfolgreich geändert.";
                errorMessage = null;
                StateHasChanged();
                await Task.Delay(2000);
                showEditModal = false;
                editTip = null;
                successMessage = null;
            }
            else
            {
                errorMessage = "Fehler beim Speichern.";
                successMessage = null;
            }

            
        }
        //new tip
            async Task CreateTip()
            {
            newTip = new TipCreateDto();
            showCreateModal = true;
            successMessageCreate = null;
            errorMessageCreate = null;

        }
        void CloseCreateModal()
        {
            showCreateModal = false;
            newTip = new TipCreateDto();
            successMessageCreate = null;
            errorMessageCreate = null;
        }

        async Task SaveNewTip()
        {
            errorMessageCreate = null;
            successMessageCreate = null;
            try
            {
                if (!string.IsNullOrEmpty(AuthService.Token))
                {
                    Httpclient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthService.Token);
                }
                var response = await Httpclient.PostAsJsonAsync("https://talentlink-9aef45cf7016.herokuapp.com/api/Tips", newTip);
                if (response.IsSuccessStatusCode)
                {
                    var created = await response.Content.ReadFromJsonAsync<Tip>();
                    if (created != null)
                    {
                        tips.Add(created);
                    }
                    successMessageCreate = "Tip erfolgreich erstellt.";
                    StateHasChanged();
                    await Task.Delay(2000);
                    showCreateModal = false;
                    newTip = new TipCreateDto();
                    successMessageCreate = null;
                }
                else
                {
                    errorMessageCreate = "Fehler beim Erstellen.";
                }
            }
            catch
            {
                errorMessageCreate = "Serverfehler beim Erstellen.";
            }
        }
    }
}
