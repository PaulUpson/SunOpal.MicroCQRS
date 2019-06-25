using System.Net.Mail;

namespace BigTree.Email
{

  public interface IEmailConverter {
    MailMessage Convert(dynamic email);
  }
}