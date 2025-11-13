namespace KimLienAdministrator.Helper.Azure.IBlob
{
    public interface IBlobService
    {
        public Task<bool> UploadAsync(IFormFile file, string filename, string extension);
        public List<string>? GetURLs(string container, string name);
    }
}
