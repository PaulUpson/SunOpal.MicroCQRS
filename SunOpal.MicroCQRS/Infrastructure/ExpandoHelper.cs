using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Linq;

namespace System {
  public static class ExpandoHelper {
     public static ExpandoObject ToExpando(this object annonymousObject) {
       IDictionary<string, object> annonymousDictionary = annonymousObject.GetType()
        .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        .ToDictionary
        (
            propInfo => propInfo.Name,
            propInfo => propInfo.GetValue(annonymousObject, null)
        );
      IDictionary<string, object> expando = new ExpandoObject();
       foreach (var item in annonymousDictionary) {
         expando.Add(item);
       }
       return (ExpandoObject) expando;
     }
  }
}