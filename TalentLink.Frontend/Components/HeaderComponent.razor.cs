namespace TalentLink.Frontend.Components
{
    public partial class HeaderComponent
    {
        public void NavToLogin()
        {
            Navi.NavigateTo("/login");
        }

        public void NavToRegister()
        {
            Navi.NavigateTo("/register");
        }

        public async Task LogoutAsync()
        {
            await AuthService.LogoutAsync();
            Navi.NavigateTo("/");
        }
        public void Dispose()
        {
            AuthService.OnAuthStateChanged -= StateHasChanged;
        }
    }
}
