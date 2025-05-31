using Company.CRUD.MVC.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Company.CRUD.MVC.PL.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            // Mail Server : gmail.com

            // Smtp (Simple Mail Transfer Protocol)
            var client = new SmtpClient("smtp.gmail.com", 587);

            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("mahmoudkhaled4812@gmail.com", "bwblczfgxcytivkb");

            client.Send("mahmoudkhaled4812@gmail.com",email.To,email.Subject,email.Body);



		}
    }
}
