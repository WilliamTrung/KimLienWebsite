using Chat.Infrastructure.Abstractions;

namespace Chat.Infrastructure.Implementations
{
    public class BaseConnectionPoolProvider : IBaseConnectionPoolProvider
    {
        private readonly Dictionary<string, List<string>> _connections = new();
        public void AddConnection(string key, string connectionId)
        {
            if (_connections.ContainsKey(key))
            {
                _connections[key].Add(connectionId);
            }
            else
            {
                _connections[key] = new List<string> { connectionId };
            }
        }

        public List<string>? GetConnection(string key)
        {
            return _connections[key];
        }

        public List<string>? GetConnection(List<string> keys)
        {
            var allConnections = new List<string>();

            if (keys == null || keys.Count == 0)
                return allConnections;

            foreach (var key in keys)
            {
                if (_connections.TryGetValue(key, out var connections) && connections is { Count: > 0 })
                {
                    allConnections.AddRange(connections);
                }
            }

            return allConnections;
        }
        public void RemoveConnection(string key)
        {
            _connections.Remove(key);
        }
    }
    public class AnonymousConnectionPoolProvider : BaseConnectionPoolProvider, IAnonymousConnectionPoolProvider
    {
    }
    public class ConnectionPoolProvider : BaseConnectionPoolProvider, IConnectionPoolProvider
    {
    }
}
