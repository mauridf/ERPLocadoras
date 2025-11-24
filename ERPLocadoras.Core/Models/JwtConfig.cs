namespace ERPLocadoras.Core.Models
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public int ExpirationInHours { get; set; }
    }
}