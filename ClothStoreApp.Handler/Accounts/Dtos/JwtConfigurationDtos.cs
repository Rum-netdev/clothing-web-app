namespace ClothStoreApp.Handler.Accounts.Dtos
{
    public class JwtConfigurationDtos
    {
        public string SecurityKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}
