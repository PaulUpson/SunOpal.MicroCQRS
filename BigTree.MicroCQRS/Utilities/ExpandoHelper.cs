using System.Collections.Generic;
using System.Dynamic;
using System.Web.Routing;

namespace System {
  public static class ExpandoHelper {
     public static ExpandoObject ToExpando(this object annonymousObject) {
       IDictionary<string, object> annonymousDictionary = new RouteValueDictionary(annonymousObject);
       IDictionary<string, object> expando = new ExpandoObject();
       foreach (var item in annonymousDictionary) {
         expando.Add(item);
       }
       return (ExpandoObject) expando;
     }
  }
}