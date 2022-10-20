namespace ScriptLoader.Core.ScriptLoaders
{
    public interface IScriptLoader
    {
        IAsyncEnumerable<Task<Script>> LoadScript(string address, CancellationToken cancellationToken = default);
    }
}
