using System;
using Version = SemanticVersioning.Version;

namespace KeySnail.Models;

public class Release
{
    public Release(string url, string name, Version semVersion, bool preRelease, DateTime releaseDate)
    {
        Url = url;
        Name = name;
        SemVersion = semVersion;
        PreRelease = preRelease;
        ReleaseDate = releaseDate;
    }

    public string Url { get; }
    public string Name { get; }
    public Version SemVersion { get; }
    public bool PreRelease { get; }
    public DateTime ReleaseDate { get; }
}