namespace BetterSync.Common.Configuration;

public class SyncConfiguration
{
    public GlobalSettings GlobalSettings { get; set; } = new();
    public List<DirectoryToSync> DirectoriesToSync { get; set; } = new();
    public List<FileToSync> FilesToSync { get; set; } = new();
}
