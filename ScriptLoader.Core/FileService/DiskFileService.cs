namespace ScriptLoader.Core.FileService
{
    public class DiskFileService : IFileService
    {
        private readonly string directoryPath;

        public DiskFileService(string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        public async Task<string> ReadFile(string fileName, CancellationToken cancellationToken = default)
        {
            using var reader = File.OpenText(Path.Combine(directoryPath, fileName));
            cancellationToken.ThrowIfCancellationRequested();
            return await reader.ReadToEndAsync();
        }

        public Task SaveFile(string fileName, string content, CancellationToken cancellationToken = default)
        {
            return File.WriteAllTextAsync(Path.Combine(directoryPath, fileName), content, cancellationToken);
        }


    }
}
