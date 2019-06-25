using System.IO;
using System.Net.Mail;
using System.Reflection;

namespace BigTree.Email
{
  public class BasicEmailSender : IEmailSender {
    public void Send(MailMessage message) {
      using (message) {
        using (var smtp = new SmtpClient()) {
          if (smtp.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
            smtp.PickupDirectoryLocation = Path.Combine(Assembly.GetExecutingAssembly().Location, "App_Data\\mail"); 
          smtp.Send(message);
        }
      }
    }
  }
}