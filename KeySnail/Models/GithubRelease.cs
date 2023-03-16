namespace KeySnail.Models;

public record GithubRelease(string tag_name, string published_at, string name, bool prerelease, string html_url);