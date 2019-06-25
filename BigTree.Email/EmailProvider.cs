using System;
using System.Net.Mail;

namespace BigTree.Email {
  /// <summary>
  /// The Email Provider seperates out the preparation of the email from the sending mechanism
  /// </summary>
  public class EmailProvider : IEmailProvider {
    private readonly IEmailConverter _emailConverter;
    private readonly IEmailSender _emailSender;
    private dynamic _email;
    private MailMessage _message;

    public EmailProvider(IEmailConverter emailConverter, IEmailSender emailSender = null) {
      _emailConverter = emailConverter;
      _emailSender = emailSender ?? new BasicEmailSender();
    }

    public void Send(dynamic email) {
      _email = email;
      _message = _emailConverter.Convert(email);
      try {
        _emailSender.Send(_message);
      }
      catch (Exception exception) {
        throw new EmailException("failed to send email : " + exception.Message);
      }
    }

    public dynamic GetEmail() {
      return _email;
    }

    public MailMessage GetMailMessage() {
      return _message;
    }
  }
}