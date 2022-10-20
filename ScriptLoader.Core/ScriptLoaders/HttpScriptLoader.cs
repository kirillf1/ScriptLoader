using HtmlAgilityPack;
using System.Net.Http;

namespace ScriptLoader.Core.ScriptLoaders
{
    public class HttpScriptLoader : IScriptLoader
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HttpScriptLoader(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async IAsyncEnumerable<Task<Script>> LoadScript(string address, CancellationToken cancellationToken = default)
        {
            var addressUri = new Uri(address);
            var scriptNodes = await GetScripts(addressUri, cancellationToken);
            if (scriptNodes == null)
                yield break;
            foreach (var scriptNode in scriptNodes)
            {
                if (cancellationToken.IsCancellationRequested)
                    yield break;
                if (scriptNode.Attributes.Contains("src"))
                {
                    yield return Task.Run(async () =>
                    {
                        var src = scriptNode.Attributes.First(c => c.Name == "src").Value;
                        Uri uri;
                        if (!src.StartsWith("http"))
                            uri = new Uri(addressUri.Host + "src");
                        else
                            uri = new Uri(src);
                        var scriptBody = await GetDoc(uri, cancellationToken);
                        return new Script(scriptBody);
                    });

                }
                else
                {
                    yield return Task.FromResult(new Script(scriptNode.InnerText));
                }
            }
        }

        private async Task<HtmlNodeCollection?> GetScripts(Uri address, CancellationToken cancellationToken)
        {
            var html = await GetDoc(address, cancellationToken);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return htmlDoc.DocumentNode.SelectNodes("//script");
        }
        private async Task<string> GetDoc(Uri address, CancellationToken cancellationToken)
        {
            using var httpClient = httpClientFactory.CreateClient();
            
            var response = await httpClient.GetAsync(address, cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Http request failed with code {(int)response.StatusCode}. Try again!");
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }
}
