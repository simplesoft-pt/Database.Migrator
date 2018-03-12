#if NET451

using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace SimpleSoft.Database.Migrator
{
    internal static class TaskEx
    {
        public static readonly Task CompletedTask = Task.FromResult(true);
    }
}

#endif
