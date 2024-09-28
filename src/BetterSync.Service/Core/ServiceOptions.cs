using BetterSync.Common.Core;

namespace BetterSync.Service.Core;

public sealed class ServiceOptions
{
    public string? ConfigFileName { get; set; }
    
    public bool Verbose { get; set; }
}
