using System;
using log4net;
using log4net.Config;

namespace BigTree.MicroCQRS
{
  public static class Logger {
    public static string Name = "BigTree";
    private static ILog _log;
    private static bool _isInitialised;

    public static void Exception(Exception exe) {
      Error(exe.ToString());
    }
    public static void Error(string message, params object[] args) {
      Error(string.Format(message, args));
    }
    public static void Error(string message) {
      Log(message, LogSeverity.Error);
    }
    public static void Warning(string message, params object[] args) {
      Warning(string.Format(message, args));
    }
    public static void Warning(string message) {
      Log(message, LogSeverity.Warning);
    }
    public static void Info(string message, params object[] args) {
      Info(string.Format(message, args));
    }
    public static void Info(string message) {
      Log(message, LogSeverity.Information);
    }
    public static void Log(string message, LogSeverity logSeverity) {
      InitialiseLogIfNeeded();
      switch (logSeverity)
      {
        case LogSeverity.Fatal:
          _log.Fatal(message);
          break;
        case LogSeverity.Error:
          _log.Error(message);
          break;
        case LogSeverity.Warning:
          _log.Warn(message);
          break;
        case LogSeverity.Information:
          _log.Info(message);
          break;
        case LogSeverity.Debug:
          _log.Debug(message);
          break;
        default:
          throw new Exception(string.Format("Severity {0} not recognised", logSeverity));
      }
    }
    private static void InitialiseLogIfNeeded() {
      if (_isInitialised) return;
      InitialiseLog(LogManager.GetLogger(Name));
      XmlConfigurator.Configure();
    }

    private static void InitialiseLog(ILog log) {
      _log = log;
      _isInitialised = true;
    }

  }
  public enum LogSeverity {
    Fatal,
    Error,
    Warning,
    Information,
    Debug
  }
}