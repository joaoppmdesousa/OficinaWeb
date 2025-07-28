using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using MailKit;
using MailKit.Net.Smtp;
using OficinaWeb.Data;
using System.Linq;

namespace OficinaWeb.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IClientRepository _clientRepository;
        private readonly IMechanicRepository _mechanicRepository;

        public EmailHelper(IConfiguration configuration, IClientRepository clientRepository, IMechanicRepository mechanicRepository)
        {
            _configuration = configuration;
            _clientRepository = clientRepository;
            _mechanicRepository = mechanicRepository;
        }


        public bool CheckEmailExists(string email, int id, bool isClient)
        {
            
            var existsInClients = _clientRepository.GetAll().Any(c => c.Email == email && (!isClient || c.Id != id));

            
            var existsInMechanics = _mechanicRepository.GetAll().Any(m => m.Email == email && (isClient || m.Id != id));

            

            return existsInClients || existsInMechanics;
        }


        public Response SendEmail(string to, string subject, string body)
        {

            var nameFrom = _configuration["Mail:NameFrom"];
            var from = _configuration["Mail:From"];
            var smtp = _configuration["Mail:Smtp"];
            var port = _configuration["Mail:Port"];
            var password = _configuration["Mail:Password"];


            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(nameFrom, from));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect(smtp, int.Parse(port), false);
                    client.Authenticate(from, password);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.ToString()
                };
            }

            return new Response
            {
                IsSuccess = true,
            };





        }
    }
}
