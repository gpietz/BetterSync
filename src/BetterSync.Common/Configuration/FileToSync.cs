namespace BetterSync.Common.Configuration;

public class FileToSync
{
    public string Source { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public bool SyncOnChange { get; set; } = false;
    public int? SyncDelay { get; set; }
    public int? SyncIntervalMinutes { get; set; }
}
