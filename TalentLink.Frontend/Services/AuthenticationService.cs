﻿using Blazored.LocalStorage;
using System.Threading.Tasks;
using TalentLink.Frontend.Models;
namespace TalentLink.Frontend.Services

{
    public class AuthenticationService

    {
        private readonly ILocalStorageService _localStorage;

        public event Action? OnAuthStateChanged;
        public bool IsAuthenticated { get; private set; }
        public string? UserName { get; private set; }
        public string? Token { get; private set; }

        public string? Email { get; private set; }
        public string Role { get; private set; }

        public Guid? VerifiedByParentId { get; set; }
        public ICollection<Job> CreatedJobs { get; set; }

        public ICollection<Rating> GivenRatings { get; private set; }

        public ICollection<Rating> ReceivedRatings { get; private set; }
        public ICollection<JobComment> WrittenComments { get; private set; }

        // NEU: Zeigt an, ob Authentifizierung geladen wurde
        public bool IsAuthLoaded { get; private set; } = false;

        public AuthenticationService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }
        public async Task SetAuthAsync(string token, string? userName, string? role, string email, ICollection<Job> createdJobs, Guid? verifiedByParentId)
        {
            Token = token;
            UserName = userName;
            Email = email;
            Role = role;
            CreatedJobs = createdJobs;
            VerifiedByParentId = verifiedByParentId;

            IsAuthenticated = true;
            await _localStorage.SetItemAsync("auth_token", token);
            await _localStorage.SetItemAsync("auth_userName", userName);
            await _localStorage.SetItemAsync("auth_role", role);
            await _localStorage.SetItemAsync("auth_email", email);
            await _localStorage.SetItemAsync("auth_createdJobs", createdJobs);
            await _localStorage.SetItemAsync("auth_verifiedByParentId", verifiedByParentId);
            OnAuthStateChanged?.Invoke();
        }
        public Task AuthLoadedTask => _authLoadedTcs.Task;
        private TaskCompletionSource _authLoadedTcs = new();

        public async Task TryRestoreAuthAsync()
        {
            Token = await _localStorage.GetItemAsync<string>("auth_token");
            UserName = await _localStorage.GetItemAsync<string>("auth_userName");
            Role = await _localStorage.GetItemAsync<string>("auth_role");
            Email = await _localStorage.GetItemAsync<string>("auth_email");
            CreatedJobs = await _localStorage.GetItemAsync<ICollection<Job>>("auth_createdJobs");
            VerifiedByParentId = await _localStorage.GetItemAsync<Guid>("auth_verifiedByParentId");
            IsAuthenticated = !string.IsNullOrEmpty(Token);
            IsAuthLoaded = true;
            _authLoadedTcs.TrySetResult();
            OnAuthStateChanged?.Invoke();
        }
        // Logout: Auth-Daten löschen
        public async Task LogoutAsync()
        {
            Token = null;
            UserName = null;
            Role = null;
            Email = null;
            IsAuthenticated = false;
            await _localStorage.RemoveItemAsync("auth_token");
            await _localStorage.RemoveItemAsync("auth_userName");
            await _localStorage.RemoveItemAsync("auth_role");
            await _localStorage.RemoveItemAsync("auth_email");
            await _localStorage.RemoveItemAsync("auth_createdJobs");
            await _localStorage.RemoveItemAsync("auth_verifiedByParentId");
            OnAuthStateChanged?.Invoke();
        }

        
    }
}
