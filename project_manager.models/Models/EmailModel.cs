using System;
using System.Collections.Generic;
using System.Text;

namespace project_manager.common.Models
{
    public class EmailModel
    {
        public EmailModel()
        {
        }

        public EmailModel(string subject, string body, string email, bool isHtml = true)
        {
            Subject = subject;
            Body = body;
            Email = email;
            IsHtml = isHtml;
        }

        public string Subject { get; set; }
        public string Body { get; set; }
        public string Email { get; set; }
        public bool IsHtml { get; set; }
    }
}
