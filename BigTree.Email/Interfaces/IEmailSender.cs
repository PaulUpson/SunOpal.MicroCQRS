using System.Net.Mail;

namespace BigTree.Email
{
  public interface IEmailSender {
    void Send(MailMessage message);
  }
}