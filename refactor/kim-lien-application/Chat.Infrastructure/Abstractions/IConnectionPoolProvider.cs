namespace Chat.Infrastructure.Abstractions
{
    public interface IBaseConnectionPoolProvider
    {
        void AddConnection(string key, string connectionId);
        void RemoveConnection(string key);
        List<string>? GetConnection(string key);
        List<string>? GetConnection(List<string> key);
    }
    public interface IAnonymousConnectionPoolProvider : IBaseConnectionPoolProvider
    {
    }
    public interface IConnectionPoolProvider : IBaseConnectionPoolProvider
    {
    }
}
