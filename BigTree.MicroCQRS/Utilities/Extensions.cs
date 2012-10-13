using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace BigTree.MicroCQRS {
  public static class Extensions
  {

    // String extensions

    public static string ToTitleCase(this string s) {
      return Regex.Replace(s, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
    }

    public static string ToTitleCase(this object e) {
      var value = e.ToString();
      return value.ToTitleCase();
    }

    public static string ToAbbreviation(this string s) {
      return Regex.Replace(s, "([a-z])", "");
    }

    public static string ToAbbreviation(this object e) {
      var value = e.ToString();
      return value.ToAbbreviation();
    }

    public static string Truncate(this string s, int charCount) {
      if (s == null) return null;
      if (s.Length <= charCount) return s;
      return s.Substring(0, charCount) + "...";
    }

    public static string StripVersioning(this string s) {
      return Regex.Replace(s, "_v\\d*", "");
    }

    public static string Replace(this string s, IEnumerable<KeyValuePair<Guid, string>> idMappings) {
      var result = s;
      idMappings.Each(m => result = s.Replace(m.Key.ToString(), m.Value));
      return result;
    }

    // Message documentation extensions

    public static void AppendWithIndent(this StringBuilder sb, string text, params object[] args) {
      sb.AppendFormat("\n\t{0}", String.Format(text, args));
    }

    public static void AppendWithDoubleIndent(this StringBuilder sb, string text, params object[] args) {
      sb.AppendFormat("\n\t\t{0}", String.Format(text, args));
    }

    public static void AppendWithIndent(this StringBuilder sb, int indentCount, string text, params object[] args) {
      var tabs = "\n";
      for (var i = 0; i < indentCount; i++) { tabs += "\t"; }
      tabs += "{0}";
      sb.AppendFormat(tabs, String.Format(text, args));
    }

    // Casting extensions

    public static TEnum AsEnum<TEnum>(this string value) where TEnum : struct {
      if (String.IsNullOrEmpty(value))
        throw new InvalidOperationException("Cannot cast null or empty string to Enum of type " +
                                            typeof(TEnum).Name);
      TEnum result;
      if (!Enum.TryParse(value, true, out result)) {
        throw new InvalidOperationException(String.Format("Cannot cast string value {0} as Enum of type {1}",
                                                          value, typeof(TEnum).Name));
      }
      return result;
    }

    public static T As<T>(this object target) {
      return (T)target;
    }

    public static int AsBit(this bool boolean) {
      return boolean ? 1 : 0;
    }

    // Date extensions

    public static bool IsBefore(this DateTime pastDate, DateTime futureDate) {
      return pastDate < futureDate;
    }

    public static int DaysAgo(this DateTime featureDate, DateTime now) {
      return (now - featureDate).Days;
    }

    public static string GetCountdown(this DateTime futureDate, DateTime now) {
      var timespan = futureDate - now;
      return string.Format("{0:00}:{1:00}:{2:00}:{3:00}", timespan.Days, timespan.Hours, timespan.Minutes, timespan.Seconds);
    }

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