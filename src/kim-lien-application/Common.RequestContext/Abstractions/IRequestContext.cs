namespace Common.RequestContext.Abstractions
{
    public interface IRequestContext
    {
        IRequestContextData Data { get; }
        void Set(IRequestContextData data);
    }
}
