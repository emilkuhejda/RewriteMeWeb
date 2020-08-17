using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RewriteMe.Business.Utils
{
    public static class ProcessingJobs
    {
        private static readonly ConcurrentDictionary<Guid, HashSet<Guid>> Jobs = new ConcurrentDictionary<Guid, HashSet<Guid>>();
        private static readonly object LockObject = new object();

        public static void Add(Guid key, Guid jobId)
        {
            lock (LockObject)
            {
                if (Jobs.ContainsKey(key))
                {
                    Jobs.TryGetValue(key, out var hashSet);
                    hashSet?.Add(jobId);
                }
                else
                {
                    Jobs.TryAdd(key, new HashSet<Guid>(new[] { jobId }));
                }
            }
        }

        public static bool AnyJob(Guid key)
        {
            lock (LockObject)
            {
                return Jobs.ContainsKey(key) && Jobs[key].Any();
            }
        }

        public static bool Exists(Guid jobId)
        {
            lock (LockObject)
            {
                return Jobs.SelectMany(x => x.Value).Contains(jobId);
            }
        }

        public static void Remove(Guid jobId)
        {
            lock (LockObject)
            {
                var keyValuePair = Jobs.SingleOrDefault(x => x.Value.Contains(jobId));
                if (keyValuePair.Value.Contains(jobId))
                {
                    keyValuePair.Value.Remove(jobId);
                }
            }
        }
    }
}
