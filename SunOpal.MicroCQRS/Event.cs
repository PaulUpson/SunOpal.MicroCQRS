using System;

namespace SunOpal.MicroCQRS
{
  public class Event : IMessage
  {
    public readonly Guid Id;
    public int Version;
    public DateTime Timestamp;
    public int UserId;

    protected Event(int userId)
    {
      Id = Guid.NewGuid();
      Timestamp = DateTime.UtcNow;
      UserId = userId;
    }
  }
}

