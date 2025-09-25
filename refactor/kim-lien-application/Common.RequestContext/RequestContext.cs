using Common.RequestContext.Abstractions;

namespace Common.RequestContext
{
    public class RequestContext : IRequestContext
    {
        private IRequestContextData? _data { get; set; }
        public IRequestContextData Data
        {
            get
            {
                if(_data is null)
                {
                    _data = new RequestContextData();
                }
                return _data;
            }
        }

        public void Set(IRequestContextData data)
        {
            _data = data;
        }
    }
}
