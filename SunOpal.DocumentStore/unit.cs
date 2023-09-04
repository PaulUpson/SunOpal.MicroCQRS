using System;
using System.Runtime.InteropServices;

namespace SunOpal.DocumentStore;

/// <summary>
/// Equivalent to System.Void which is not allowed to be used in the code for some reason.
/// </summary>
[ComVisible(true)]
[Serializable]
[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct unit
{
  public static readonly unit it = default;
}