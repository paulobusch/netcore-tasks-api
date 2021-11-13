namespace Tasks.Domain._Common.Security
{
    public class TokenConfiguration
    {
        public string Issuer { get; set; }
        public string Signature { get; set; }
        public int Seconds { get; set; }
    }
}
