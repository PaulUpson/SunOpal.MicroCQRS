using System;
using System.IO;
using System.Reflection;

namespace SunOpal.MicroCQRS
{
  public static class NumericExtensions
  { 
    // Numeric extensions

    /// <summary>
    /// Returns true if the given integer is between the two numbers provided. 
    /// NOTE: this is an inclusive range. For an exclusive check adjust the two range values.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="lower"></param>
    /// <param name="higher"></param>
    /// <returns></returns>
    public static bool Between(this int target, int lower, int higher) {
      return target >= lower && target <= higher;
    }

    // Filepath extensions

    public static string GetCurrentExecutingDirectory() {
      return Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
    }
  }
}