using System;

namespace BigTree.MicroCQRS
{
  public interface Message { }

  public class Command : Message
  {
    public readonly int UserId;
    public int OriginalVersion;

    public Command(int userId)
    {
      UserId = userId;
    }
  }

  public class Event : Message
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

