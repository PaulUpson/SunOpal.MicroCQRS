using System.Net.Mail;

namespace SunOpal.Email
{
  public interface IEmailSender {
    void Send(MailMessage message);
  }
}