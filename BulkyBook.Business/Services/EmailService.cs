using BulkyBook.Business.Services.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.Business.Services
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _secretKey;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public EmailService(IConfiguration configuration)
        {
            _configuration= configuration;
            _apiKey = _configuration["Mailjet:ApiKey"];
            _secretKey = _configuration["Mailjet:SecretKey"];
            _senderEmail = _configuration["Mailjet:SenderEmail"];
            _senderName = _configuration["Mailjet:SenderName"];
        }


        public Task<bool> SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendOrderConfirmationEmailAsync(string toEmail, int orderId, decimal orderTotal)
        {
            throw new NotImplementedException();
        }
    }
}
