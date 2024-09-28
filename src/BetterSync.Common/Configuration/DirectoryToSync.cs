namespace BetterSync.Common.Configuration;

public class DirectoryToSync
{
    public string Source { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public bool IncludeSubdirectories { get; set; } = true; 
    public bool SyncOnChange { get; set; } = false; 
    public int? SyncIntervalMinutes { get; set; } 
    public bool? EnableGitIgnore { get; set; } 
    public string? FileMask { get; set; }
    public string? ExcludeMask { get; set; }  
}
