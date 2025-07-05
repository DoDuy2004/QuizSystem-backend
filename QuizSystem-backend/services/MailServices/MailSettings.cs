namespace QuizSystem_backend.services.MailServices
{
    public class MailSettings
    {
        public MailSettings() { }
        public string? Mail { get; set; }
        public string? Displayname { get; set; }
        public string? Password { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; } = 1;
    }
}
