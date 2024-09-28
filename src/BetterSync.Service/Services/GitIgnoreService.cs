using System.Text.RegularExpressions;

namespace BetterSync.Service.Services;

internal sealed class GitIgnoreService
{
    private readonly string _gitIgnorePath;
    private readonly List<string> _ignorePatterns = new();

    public GitIgnoreService(string gitIgnorePath)
    {
        _gitIgnorePath = gitIgnorePath;
        LoadGitIgnore();        
    }

    private void LoadGitIgnore()
    {
        if (!File.Exists(_gitIgnorePath)) 
            return;
        
        var lines = File.ReadAllLines(_gitIgnorePath);
        _ignorePatterns.AddRange(
            lines
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrWhiteSpace(l) && !l.StartsWith("#"))
        );
    }

    public bool ShouldIgnore(string filePath)
    {
        if (filePath == null) 
            throw new ArgumentNullException(nameof(filePath));
        
        return _ignorePatterns.Any(pattern => WildcardMatch(filePath, pattern));
    }
    
    private static bool WildcardMatch(string input, string pattern)
    {
        return Regex.IsMatch(input, "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$");
    }
}
