using System;

namespace SunOpal.DocumentStore;

public record DocumentRecord
{
  /// <summary>
  /// Path of the view in the subfolder, using '/' as split on all platforms
  /// </summary>
  public readonly string Key;
  public readonly Func<byte[]> Read;

  public DocumentRecord(string key, Func<byte[]> read)
  {
    Key = key;
    Read = read;
  }
}