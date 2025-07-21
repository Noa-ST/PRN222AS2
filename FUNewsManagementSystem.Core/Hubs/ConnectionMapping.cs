using System;
using System.Collections.Concurrent;

namespace FUNewsManagementSystem.Core.Hubs
{
    public static class ConnectionMapping
    {
        private static readonly ConcurrentDictionary<string, HashSet<string>> _connections = new();

        public static void Add(string userId, string connectionId)
        {
            _connections.AddOrUpdate(userId,
                new HashSet<string> { connectionId },
                (key, oldSet) => { oldSet.Add(connectionId); return oldSet; });
        }

        public static void Remove(string userId, string connectionId)
        {
            if (_connections.TryGetValue(userId, out var connections))
            {
                connections.Remove(connectionId);
                if (connections.Count == 0)
                {
                    _connections.TryRemove(userId, out _);
                }
            }
        }

        public static List<string> GetAllExcept(string excludeUserId)
        {
            return _connections
                .Where(kv => kv.Key != excludeUserId)
                .SelectMany(kv => kv.Value)
                .ToList();
        }
    }
}
