namespace KimLienAdministrator.Helper.Azure.IBlob
{
    public interface IBlobService
    {
        public Task<bool> UploadAsync(List<IFormFile> files, string container, int id);
        public List<string>? GetURLs(string container, int id);
    }
}
