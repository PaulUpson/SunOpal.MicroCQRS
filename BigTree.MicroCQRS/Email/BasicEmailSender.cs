using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Web;

namespace BigTree.MicroCQRS.Email {
  public class BasicEmailSender : IEmailSender {
    public void Send(MailMessage message) {
      using (message) {
        using (var smtp = new SmtpClient()) {
          if (smtp.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
            smtp.PickupDirectoryLocation = Path.Combine(HttpRuntime.AppDomainAppPath, ConfigurationManager.AppSettings.Get("mailPickupDirectory")); 
          smtp.Send(message);
        }
      }
    }
  }
}