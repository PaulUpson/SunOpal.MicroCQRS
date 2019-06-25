namespace BigTree.MicroCQRS
{
  public class Command : IMessage
  {
    public readonly int UserId;
    public int OriginalVersion;

    public Command(int userId)
    {
      UserId = userId;
    }
  }
}

