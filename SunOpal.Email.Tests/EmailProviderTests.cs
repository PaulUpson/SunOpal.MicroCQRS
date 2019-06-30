using SunOpal.Email;
using NUnit.Framework;
using System.Net.Mail;
using System.Dynamic;
using System;

namespace Tests
{
  public class EmailProviderTests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SendIsCalledIfEmailIsValid()
    {
      var mockSender = new FakeEmailSender();
      var sut = new EmailProvider(emailSender:mockSender);
      dynamic testEmail = new ExpandoObject();

      testEmail.From = "from.no.one@nowhere.com";
      testEmail.To = "to.no.one@nowhere.com";
      testEmail.Subject = "NADA";
      testEmail.Body = "No Body";
      
      sut.Send(testEmail);

      Assert.That(mockSender.SendWasCalled, Is.True);
    }

    [Test]
    public void FailureToSendThrowsException()
    {
      var mockSender = new FakeEmailSender();
      mockSender.WillFail = true;

      var sut = new EmailProvider(emailSender: mockSender);
      dynamic testEmail = new ExpandoObject();

      testEmail.From = "from.no.one@nowhere.com";
      testEmail.To = "to.no.one@nowhere.com";
      testEmail.Subject = "NADA";
      testEmail.Body = "No Body";

      Assert.Throws<EmailException>(() => sut.Send(testEmail));
    }

    public class FakeEmailSender : IEmailSender
    {
      public bool SendWasCalled { get; set; }
      public bool WillFail { get; set; }

      public void Send(MailMessage message)
      {
        if (WillFail) throw new Exception("failed to send");

        SendWasCalled = true;
      }
    }
  }
}