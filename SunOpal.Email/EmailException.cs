using System;

namespace SunOpal.Email
{
  public class EmailException : Exception
  {
    public EmailException(string message) : base(message) { }
  }
}
