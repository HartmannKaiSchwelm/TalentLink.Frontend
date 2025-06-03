using System.Net.Http.Json;
using TalentLink.Frontend.Models;

namespace TalentLink.Frontend.Pages
{
    public partial class TipsPage
    {
        private List<Tip> tips = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {

                var response = await Httpclient.GetAsync("https://localhost:7024/api/Tips");
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
    }
}
