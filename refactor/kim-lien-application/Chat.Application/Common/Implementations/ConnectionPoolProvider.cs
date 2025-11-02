using Chat.Application.Common.Abstractions;

namespace Chat.Application.Common.Implementations
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
