namespace Ap.Email.Models
{
    public class EmailModel
    {
        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string[] Attachmets { get; set; }
    }
}