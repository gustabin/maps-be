using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Isban.Maps.Entity.Helpers
{
    public class Cache
    {
        private static List<string> NameCache = new List<string>();

        public static T Get<T>(string cacheKey, Func<T> func, int timeoutHours = 1, int timeoutMinutes = 0)
        {
            if (MemoryCache.Default.Contains(cacheKey))
            {
                return (T)MemoryCache.Default.Get(cacheKey);
            }
            var result = func.Invoke();

            if (result != null)
            {
                MemoryCache.Default.Add(cacheKey, result, DateTimeOffset.Now.AddHours(timeoutHours).AddMinutes(timeoutMinutes));
                NameCache.Add(cacheKey);
            }

            return result;
        }

        public static void Clear()
        {
            foreach (var cacheKey in NameCache)
            {
                if (MemoryCache.Default.Contains(cacheKey))
                {
                    MemoryCache.Default.Remove(cacheKey);
                }
            }
            NameCache.Clear();
        }
    }
}
