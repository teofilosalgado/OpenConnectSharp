namespace OpenConnectSharp.Domain.Models
{
    public class Form
    {
        public string Gateway { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool SaveCredentials { get; set; } = false;
    }
}
