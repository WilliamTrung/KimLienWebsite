namespace Chat.Application.Common.Abstractions
{
    public interface IBaseConnectionPoolProvider
    {
        void AddConnection(string key, string connectionId);
        void RemoveConnection(string key);
        List<string>? GetConnection(string key);
    }
    public interface IAnonymousConnectionPoolProvider : IBaseConnectionPoolProvider
    {
    }
    public interface IConnectionPoolProvider : IBaseConnectionPoolProvider
    {
    }
}
