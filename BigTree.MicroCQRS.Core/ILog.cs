using System;

namespace BigTree.MicroCQRS
{
  public interface ILog
  {
    void Exception(Exception innerException);
    void Error(string message, params object[] args);
    void Error(string message);
    void Warning(string message, params object[] args);
    void Warning(string message);
    void Info(string message, params object[] args);
    void Info(string message);
    void Log(string message, LogSeverity logSeverity);
  }

  public enum LogSeverity
  {
    Fatal,
    Error,
    Warning,
    Information,
    Debug
  }
}