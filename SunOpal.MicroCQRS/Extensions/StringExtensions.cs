using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SunOpal.MicroCQRS
{
  public static class StringExtensions
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
  }
}