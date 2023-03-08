using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace KeySnail.Config;

public static class ConfigManager
{
    public static string FolderPath { get; private set; } = GetDefaultFolderPath();

    public static string GetLocalFilePath(string filename)
    {
        return Path.Combine(FolderPath, filename);
    }

    private static string GetDefaultFolderPath()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var companyName = Assembly.GetEntryAssembly().GetCustomAttributes<AssemblyCompanyAttribute>().FirstOrDefault();
        var folderName = (companyName?.Company ?? Assembly.GetEntryAssembly()?.GetName().Name);

        if (folderName is null)
        {
            throw new Exception("Could not determine company name");
        }
        
        return Path.Combine(appData, folderName);
    }
    
    public static void SetFolderName(string folderName)
    {
        FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), folderName);
    }
}