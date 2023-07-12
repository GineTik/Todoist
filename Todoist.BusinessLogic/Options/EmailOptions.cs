namespace Todoist.BusinessLogic.Options
{
    public class EmailOptions
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public EmailCredentials Credentials { get; set; } = default!;
    }

    public class EmailCredentials
    {
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
