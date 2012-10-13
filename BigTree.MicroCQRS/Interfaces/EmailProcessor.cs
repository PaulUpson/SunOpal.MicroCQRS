using System.Net.Mail;

namespace BigTree.MicroCQRS
{
  public interface IEmailProvider {
    void Send(dynamic email);
    dynamic GetEmail();
    MailMessage GetMailMessage();
  }

  public interface IEmailSender {
    void Send(MailMessage message);
  }

  public interface IEmailConverter {
    MailMessage Convert(dynamic email);
  }
}