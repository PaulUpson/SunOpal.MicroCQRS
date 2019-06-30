using System.Net.Mail;

namespace SunOpal.Email
{
  public interface IEmailProvider {
    void Send(dynamic email);
    dynamic GetEmail();
    MailMessage GetMailMessage();
  }
}