using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using TalentLink.Frontend.Services;

namespace TalentLink.Frontend.Components
{
    public partial class HeaderComponent : IDisposable
    {
        private bool navOpen = false;
        protected override void OnInitialized()
        {
            Console.WriteLine($"HeaderComponent OnInitialized: navOpen = {navOpen}");
            AuthService.OnAuthStateChanged += StateHasChanged;
            Navi.LocationChanged += OnLocationChanged;
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
        
        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            if (navOpen)
            {
                navOpen = false;
                InvokeAsync(StateHasChanged);
            }
        }
        
        public void ToggleNav() 
        {
            navOpen = !navOpen;
            Console.WriteLine($"ToggleNav called: navOpen = {navOpen}");
        }
        public void CloseNav() => navOpen = false;
        
        public void NavigateAndClose(string url)
        {
            navOpen = false;
            StateHasChanged();
            Navi.NavigateTo(url);
        }
        
        public async Task LogoutAndClose()
        {
            navOpen = false;
            await LogoutAsync();
        }
        
        public void LoginAndClose()
        {
            navOpen = false;
            StateHasChanged();
            NavToLogin();
        }
        
        public void RegisterAndClose()
        {
            navOpen = false;
            StateHasChanged();
            NavToRegister();
        }
        
        public void Dispose()
        {
            AuthService.OnAuthStateChanged -= StateHasChanged;
            Navi.LocationChanged -= OnLocationChanged;
        }
    }
}
