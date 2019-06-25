using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BigTree.MicroCQRS
{
  static class NameCache<T>
  {
    public static readonly string Name;
    public static readonly string Namespace;

    static NameCache()
    {
      var type = typeof(T);

      Name = new string(Splice(type.Name).ToArray()).TrimStart('-');
      var dcs = type.GetCustomAttributes(false).OfType<DataContractAttribute>().ToArray();

      if (dcs.Length <= 0) return;
      var attribute = dcs.First();

      if (!string.IsNullOrEmpty(attribute.Name))
      {
        Name = attribute.Name;
      }

      if (!string.IsNullOrEmpty(attribute.Namespace))
      {
        Namespace = attribute.Namespace;
      }
    }

    static IEnumerable<char> Splice(string source)
    {
      foreach (var c in source)
      {
        if (char.IsUpper(c))
        {
          yield return '-';
        }
        yield return char.ToLower(c);
      }
    }
  }
}