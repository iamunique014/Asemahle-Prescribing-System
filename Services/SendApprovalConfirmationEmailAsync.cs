//using PrescribingSystem.Models;
//using System.Net;
//using System.Net.Mail;

//namespace PrescribingSystem.Services
//{
//    private async Task SendApprovalConfirmationEmailAsync(string toEmail, string stockOrderNumber)
//    {
//        var mail = new MailMessage
//        {
//            From = new MailAddress(SmtpSettings, "Pharmacy System"),
//            Subject = $"Stock Order {stockOrderNumber} Approved",
//            Body = $"Dear user,\n\nStock order #{stockOrderNumber} has been approved.\n\nRegards,\nPharmacy System",
//            IsBodyHtml = false
//        };

//        mail.To.Add(toEmail);

//        using var smtp = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
//        {
//            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
//            EnableSsl = _smtpSettings.EnableSSL
//        };

//        await smtp.SendMailAsync(mail);
//    }

//}
