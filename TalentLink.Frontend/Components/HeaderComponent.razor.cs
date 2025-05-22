namespace TalentLink.Frontend.Components
{
    public partial class HeaderComponent
    {
        protected override void OnInitialized()
        {
            AuthService.OnAuthStateChanged += StateHasChanged;
        }

        public void NavToRegister()
        {
            Navi.NavigateTo("/register");
        }
        public void NavToLogin()
        {
            Navi.NavigateTo("/login");
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
