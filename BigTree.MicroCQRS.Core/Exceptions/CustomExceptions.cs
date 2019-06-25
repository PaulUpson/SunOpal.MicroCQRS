using System;

namespace BigTree.MicroCQRS
{
  public class SystemInitialisationException : Exception
  {
    public SystemInitialisationException(string message, Exception innerException)
      : base(message, innerException) { }
  }

  public class MessageSendingException : Exception
  {
    public MessageSendingException(string message, Exception innerException)
      : base(message, innerException) { }
  }

  public class AggregateNotFoundException : Exception { }

  public class ConcurrencyException : Exception { }
}
