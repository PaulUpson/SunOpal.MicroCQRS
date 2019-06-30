using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace SunOpal.Email
{
  public class BasicEmailConverter : IEmailConverter
  {
    public MailMessage Convert(dynamic email)
    {
      return new MailMessage(
        email.From ?? "no.one@nowhere.com", 
        email.To is IEnumerable<string> tolist ? tolist.First() : email.To ?? "no-one@nowhere.com", 
        email.Subject ?? "Nada", 
        email.Body ?? "nothing");
    }
  }
}