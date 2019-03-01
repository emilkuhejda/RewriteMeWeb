namespace RewriteMe.Domain.Settings
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }

        public string SecretKey { get; set; }

        public SpeechCredentials SpeechCredentials { get; set; }
    }
}
