using System;
using System.Collections.Generic;
using System.Linq;

namespace SunOpal.MicroCQRS {
  public static class CollectionExtensions {
    public static T Second<T>(this IEnumerable<T> collection)
    {
      return collection.Skip(1).First();
    }

    public static T Third<T>(this IEnumerable<T> collection)
    {
      return collection.Skip(2).First();
    }

    public static T Fourth<T>(this IEnumerable<T> collection)
    {
      return collection.Skip(3).First();
    }

    public static T Fifth<T>(this IEnumerable<T> collection)
    {
      return collection.Skip(4).First();
    }

    public static T Sixth<T>(this IEnumerable<T> collection)
    {
      return collection.Skip(5).First();
    }

    public static T Seventh<T>(this IEnumerable<T> collection)
    {
      return collection.Skip(6).First();
    }

    public static T Eighth<T>(this IEnumerable<T> collection)
    {
      return collection.Skip(7).First();
    }

    public static void Each<T>(this IEnumerable<T> collection, Action<T> action)
    {
      foreach (var item in collection) {
        action(item);
      }
    }

    public static IEnumerable<T> Except<T>(this IEnumerable<T> collection, params T[] singles) {
      return collection.Except((IEnumerable<T>) singles);
    } 
  }
}