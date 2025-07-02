namespace TalentLink.Frontend
{
    public class ApiConfig
    {
        public string ApiBaseUrl { get; set; } = "https://localhost:7024/api";
        public StripeConfig Stripe { get; set; } = new StripeConfig();
    }
    
    public class StripeConfig
    {
        public string PublishableKey { get; set; } = "";
    }
}
