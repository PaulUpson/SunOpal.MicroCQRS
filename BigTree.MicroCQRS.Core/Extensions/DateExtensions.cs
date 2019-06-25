using System;

namespace BigTree.MicroCQRS
{
  public static class DateExtensions
  {
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
  }
}