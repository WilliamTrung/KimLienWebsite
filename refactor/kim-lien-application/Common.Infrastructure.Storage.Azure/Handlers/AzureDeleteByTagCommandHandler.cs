namespace Common.Infrastructure.Storage.Azure.Handlers
{
    public class AzureDeleteByTagCommandHandler : AzureStorageBaseHandler, MediatR.IRequestHandler<Commands.AzureDeleteByTagCommand, int>
    {
        public AzureDeleteByTagCommandHandler(IServiceProvider service) : base(service)
        {
        }
        public async Task<int> Handle(Commands.AzureDeleteByTagCommand request, CancellationToken cancellationToken)
        {
            return await _storageService.DeleteByTagsAsync(request.Tags, request.MatchAny, cancellationToken);
        }
    }
}
