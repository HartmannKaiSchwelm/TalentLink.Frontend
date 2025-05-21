namespace TalentLink.Frontend.Services
{
    public class AuthenticationService
    {
        public event Action? OnAuthStateChanged;
        public bool IsAuthenticated { get; private set; }
        public string? UserName { get; private set; }
        public string? Token { get; private set; }

        public void SetAuth(string token, string? userName)
        {
            Token = token;
            UserName = userName;
            IsAuthenticated = true;
            OnAuthStateChanged?.Invoke();
        }

        public void Logout()
        {
            Token = null;
            UserName = null;
            IsAuthenticated = false;
            OnAuthStateChanged?.Invoke();
        }
    }
}
