using System.Reflection;

namespace UniqueInstances.Extensions
{
    public static partial class Extensions
    {
        private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    }
}
