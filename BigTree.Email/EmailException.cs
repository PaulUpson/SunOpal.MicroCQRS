using System;

namespace BigTree.Email
{
  public class EmailException : Exception
  {
    public EmailException(string message) : base(message) { }
  }
}
