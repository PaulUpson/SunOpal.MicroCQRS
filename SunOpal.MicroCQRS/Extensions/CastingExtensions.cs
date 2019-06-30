using System;

namespace SunOpal.MicroCQRS
{
  public static class CastingExtensions
  {
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
  }
}