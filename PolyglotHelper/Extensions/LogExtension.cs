using System.Diagnostics;

namespace PolyglotHelper.Extensions;

public static class DebugExtension
{
    public static void Log<T>(this T _, string message) => Debug.WriteLine("\t" + message, typeof(T).Name);
}
