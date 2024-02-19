namespace ClothStoreApp.Share.Options
{
    public class JwtSecurityConfiguration
    {
        public string SecurityKey { get; set; }
        public string SigningKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
