using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Your email sending logic here
            // Example: Send email using an email service or library
            // For simplicity, you can use Console.WriteLine to simulate sending an email
            Console.WriteLine($"Sending email to {email} with subject: {subject}");
            return Task.CompletedTask;
        }
    }
}
