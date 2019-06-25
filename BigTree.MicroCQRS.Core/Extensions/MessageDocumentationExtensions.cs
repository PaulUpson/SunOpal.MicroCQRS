using System;
using System.Text;

namespace BigTree.MicroCQRS
{
  public static class MessageDocumentationExtensions
  {
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
  }
}