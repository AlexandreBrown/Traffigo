using System.Net.Mail;
using System.IO;
using TraffiGo.Modeles.ClasseSql;
using TraffiGo.Modeles.Data;
using System.Security;

namespace TraffiGo.Modeles
{
    public static class Courriel
    {
        public static void SendEmail(string courriel)
        {
            string newPassword = Generator.RandomPassword();
            Utilisateur utilisateur = MySqlUtilisateurs.RetrieveWithCourriel(courriel);
            utilisateur.ModifierMotPasse(newPassword);

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.googlemail.com", 587);

            mail.From = new MailAddress("putYourEmailHere@gmail.com");
            mail.To.Add(courriel);
            mail.Subject = "Problème de connexion?";
            var path = Directory.GetCurrentDirectory();
            string HTML = File.ReadAllText(path + "\\Resources\\HTML\\Courriel.html");
            HTML = HTML.Replace("{{user.name}}", courriel);
            HTML = HTML.Replace("{{user.password}}", newPassword);
            mail.IsBodyHtml = true;
            mail.Body = HTML;

            using (var secureP = new SecureString())
            {
                var p = "putYourEmailPasswordHere";
                foreach (var c in p.ToCharArray())
                {
                    secureP.AppendChar(c);
                }
                secureP.MakeReadOnly();
            SmtpServer.Credentials = new System.Net.NetworkCredential("putYourEmailHere@gmail.com", secureP);
            }

            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
    }
}
