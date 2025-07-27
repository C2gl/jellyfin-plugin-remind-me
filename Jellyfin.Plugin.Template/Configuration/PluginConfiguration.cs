using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Template.Configuration;

/// <summary>
/// Plugin configuration.
/// </summary>
public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginConfiguration"/> class.
    /// </summary>
    public PluginConfiguration()
    {
        // Set default options
        EnableAutoQueue = true;
        RequireFullWatchPercentage = 80;
        OnlyForCollections = false;
        DelayMinutes = 0;
    }

    /// <summary>
    /// Gets or sets a value indicating whether auto-queuing is enabled.
    /// </summary>
    public bool EnableAutoQueue { get; set; }

    /// <summary>
    /// Gets or sets the percentage of movie that must be watched to trigger next movie queuing.
    /// </summary>
    public int RequireFullWatchPercentage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to only work with items in collections.
    /// </summary>
    public bool OnlyForCollections { get; set; }

    /// <summary>
    /// Gets or sets delay in minutes before adding next movie to continue watching.
    /// </summary>
    public int DelayMinutes { get; set; }
}

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string AString { get; set; }

    /// <summary>
    /// Gets or sets an enum option.
    /// </summary>
    public SomeOptions Options { get; set; }
}
