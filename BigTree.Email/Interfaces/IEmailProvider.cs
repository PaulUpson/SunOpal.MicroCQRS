using System.Net.Mail;

namespace BigTree.Email
{
  public interface IEmailProvider {
    void Send(dynamic email);
    dynamic GetEmail();
    MailMessage GetMailMessage();
  }
}