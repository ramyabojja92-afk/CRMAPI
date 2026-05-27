using CRMFunction0.Entities;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace CRMFunction0.Services
{
    public class EmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _config;

        public EmailService(
            ILogger<EmailService> logger,
            IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task SendEmailAsync(Customer customer)
        {
            var username = _config["MailtrapUsername"];
            var password = _config["MailtrapPassword"];

            var email = new MimeMessage();

            email.From.Add(
                MailboxAddress.Parse("crm@test.com"));

            email.To.Add(
                MailboxAddress.Parse(customer.Seller.Email));

            email.Subject = "New Customer Assigned";

            email.Body = new TextPart("plain")
            {
                Text =
                $"""
                Hello {customer.Seller.Name},

                You are now responsible for a new customer.

                Customer Information:

                Name: {customer.Name}
                Title: {customer.Title}
                Phone: {customer.Phone}
                Email: {customer.Email}
                Address: {customer.Address}

                Regards,
                CRM System
                """
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            await smtp.ConnectAsync(
                "sandbox.smtp.mailtrap.io",
                2525,
                SecureSocketOptions.StartTls);

            if (string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password))
            {
                throw new Exception("Mailtrap credentials missing");
            }

            await smtp.AuthenticateAsync(username, password);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);

            _logger.LogInformation(
                $"Email sent to {customer.Seller.Email}");
        }
    }
}