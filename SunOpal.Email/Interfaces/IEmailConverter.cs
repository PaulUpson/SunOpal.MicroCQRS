using System.Net.Mail;

namespace SunOpal.Email
{

  public interface IEmailConverter {
    MailMessage Convert(dynamic email);
  }
}