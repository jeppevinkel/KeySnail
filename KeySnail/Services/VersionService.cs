using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using KeySnail.Models;
using Version = SemanticVersioning.Version;

namespace KeySnail.Services;

public interface IVersionService
{
    Release? GetLatestVersion(bool preRelease = false);
    Task<Release?> GetLatestVersionAsync(bool preRelease = false);
}

public class GitHubVersionService: IVersionService
{
    private readonly HttpClient _httpClient;

    public GitHubVersionService()
    {
        _httpClient = new();
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "KeySnail Repository Reporter");
    }
    
    public Release? GetLatestVersion(bool preRelease = false)
    {
        var res = GetLatestVersionAsync(preRelease).Result;
        Debug.WriteLine(res);
        return res;
    }
    
    public async Task<Release?> GetLatestVersionAsync(bool preRelease = false)
    {
        var res = await ProcessLatestReleaseAsync().ConfigureAwait(false);
        Debug.WriteLine("HMmmm");
        return res;
    }

    private async Task<Release?> ProcessLatestReleaseAsync()
    {
        await using var stream =
            await _httpClient.GetStreamAsync("https://api.github.com/repos/jeppevinkel/KeySnail/releases/latest").ConfigureAwait(false);
        Debug.WriteLine("LOLLEREN");
        var release =
            await JsonSerializer.DeserializeAsync<GithubRelease>(stream);
        
        Debug.WriteLine(release);

        return new Release(release.html_url, release.name, new Version(release.tag_name), release.prerelease, DateTime.Parse(release.published_at));
        return release is null ? null : new Release(release.html_url, release.name, new Version(release.tag_name), release.prerelease, DateTime.Parse(release.published_at));
    }
}