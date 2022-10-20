namespace ScriptLoader.Core.FileService
{
    public interface IFileService
    {
        public Task SaveFile(string fileName, string content, CancellationToken cancellationToken = default);
        public Task<string> ReadFile(string fileName, CancellationToken cancellationToken = default);
    }
}
