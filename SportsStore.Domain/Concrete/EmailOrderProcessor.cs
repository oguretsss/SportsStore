using SportsStore.Domain.Abstract;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using SportsStore.Domain.Entities;
using System.IO;
using System.Diagnostics;

namespace SportsStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "voronin-anton1@yandex.ru";
        public string MailFromAddress = "hestheone30@gmail.com";
        public string MailFromName = "Tophail";
        public bool  UseSSL = true;
        public string Username = "hestheone30@gmail.com";
        public string Password = "242byobmf";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"D:\TestMails";
    }
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings settings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            this.settings = settings;
        }
        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = settings.UseSSL;
                smtpClient.Host = settings.ServerName;
                smtpClient.Port = settings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(settings.Username, settings.Password);
                if(settings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = settings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("A new order was submitted")
                    .AppendLine("---")
                    .AppendLine("Items: ");

                foreach(var line in cart.Lines)
                {
                    var subTotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} X {1} (subtotal: {2:c})\n", line.Quantity, line.Product.Name, subTotal);
                }

                body.AppendFormat("Total order value: {0:c}\n", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.AddressLine1)
                    .AppendLine(shippingDetails.AddressLine2 ?? "")
                    .AppendLine(shippingDetails.AddressLine3 ?? "")
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.State ?? "")
                    .AppendLine(shippingDetails.Country)
                    .AppendLine(shippingDetails.ZipCode)
                    .AppendLine("---")
                    .AppendFormat("Gift wrap: {0}", (shippingDetails.GiftWrap ? "Yes" : "No"));

                MailMessage message = new MailMessage(
                    settings.MailFromAddress, //From
                    settings.MailToAddress, //To
                    "You have a new order!", //Subj
                    body.ToString()
                    );

                if (settings.WriteAsFile) message.BodyEncoding = Encoding.UTF8;


                try
                {
                    smtpClient.Send(message);
                    //Debug.WriteLine("TORPHIK! " + settings.MailToAddress + " successfully sent mail");
                    Debug.WriteLine("TORPHIK! " + settings.FileLocation + " successfully sent mail");
                }
                catch (SmtpException e)
                { Debug.WriteLine("TORPHIK! " + e.Message); }
            }
        }
    }
}
