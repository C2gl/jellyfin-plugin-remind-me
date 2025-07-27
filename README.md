# Next in Series Plugin

[![License](https://img.shields.io/github/license/C2gl/jellyfin-plugin-remind-me)](LICENSE)
[![Release](https://img.shields.io/github/v/release/C2gl/jellyfin-plugin-remind-me)](https://github.com/C2gl/jellyfin-plugin-remind-me/releases)
[![Jellyfin Version](https://img.shields.io/badge/jellyfin-10.9%2B-blue)](https://jellyfin.org/)

A Jellyfin plugin that automatically queues the next movie in a series or collection to your Continue Watching list when you finish watching a movie. Perfect for movie marathons and binge-watching series!

## Features

- 🎬 **Automatic Queuing**: Detects when you finish watching a movie and automatically adds the next movie in the series to Continue Watching
- ⚙️ **Configurable Watch Threshold**: Set the percentage of the movie that must be watched before triggering (default: 80%)
- 📚 **Collection Support**: Option to only work with movies that are part of collections
- ⏱️ **Delayed Queuing**: Add an optional delay before queuing the next movie
- 🎯 **Smart Detection**: Automatically finds the next movie in chronological order within collections

## Installation

### Method 1: Plugin Repository (Recommended)

1. In Jellyfin, go to **Dashboard** → **Plugins** → **Repositories**
2. Add this repository URL: `https://raw.githubusercontent.com/C2gl/jellyfin-plugin-remind-me/master/manifest.json`
3. Go to **Catalog** and install "Next in Series"
4. Restart Jellyfin

### Method 2: Manual Installation

1. Download the latest release from the [Releases page](https://github.com/C2gl/jellyfin-plugin-remind-me/releases)
2. Extract the `.zip` file to your Jellyfin plugins directory:
   - **Windows**: `%PROGRAMDATA%\Jellyfin\Server\plugins\NextInSeries\`
   - **Linux**: `/var/lib/jellyfin/plugins/NextInSeries/`
   - **Docker**: `/config/plugins/NextInSeries/`
3. Restart Jellyfin

## Configuration

After installation, configure the plugin:

1. Go to **Dashboard** → **Plugins** → **Next in Series**
2. Adjust the settings according to your preferences:

### Settings

- **Enable Auto-Queue Next Movie**: Master toggle to enable/disable the plugin
- **Watch Percentage Required** (0-100): Percentage of movie that must be watched before queuing the next movie (default: 80%)
- **Only Work with Collections**: If enabled, only movies that are part of collections will trigger auto-queuing
- **Delay Before Queuing** (minutes): Optional delay before adding the next movie to Continue Watching (default: 0)
## How It Works

1. **Movie Completion Detection**: The plugin monitors your watch progress and detects when you've watched enough of a movie (configurable percentage)
2. **Series/Collection Identification**: It identifies if the watched movie is part of a collection or series
3. **Next Movie Discovery**: Finds the next movie in chronological order within the collection
4. **Auto-Queue**: Adds the next movie to your Continue Watching list, optionally with a delay

## Requirements

- Jellyfin 10.9.0 or higher
- Movies organized in Collections for best results
- .NET 8.0 runtime (included with Jellyfin)

## Usage

Once installed and configured, the plugin works automatically in the background:

1. Watch a movie that's part of a collection
2. When you reach the configured watch percentage, the plugin detects completion
3. The next movie in the collection is automatically added to Continue Watching
4. Continue your marathon from the Continue Watching section!

## Example Scenarios

- **Marvel Cinematic Universe**: Finish "Iron Man" → "The Incredible Hulk" appears in Continue Watching
- **Lord of the Rings**: Complete "Fellowship of the Ring" → "The Two Towers" is queued automatically
- **TV Movie Series**: Finish the first TV movie → Second movie in the series is ready to watch

## Troubleshooting

### Plugin not working?

1. **Check Configuration**: Ensure the plugin is enabled in settings
2. **Verify Collections**: Make sure your movies are properly organized in collections
3. **Watch Percentage**: Verify you've watched enough of the movie (check your percentage threshold)
4. **Logs**: Check Jellyfin logs for any plugin-related errors

### Common Issues

- **Movies not in collections**: If "Only Work with Collections" is enabled, movies must be part of a collection
- **Watch threshold too high**: Lower the required watch percentage if movies aren't being detected as "finished"
- **Missing metadata**: Ensure movies have proper metadata and are correctly identified by Jellyfin

## Building from Source

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git

### Steps

1. **Clone the repository**:
   ```powershell
   git clone https://github.com/C2gl/jellyfin-plugin-remind-me.git
   cd jellyfin-plugin-remind-me
   ```

2. **Build the plugin**:
   ```powershell
   dotnet build --configuration Release
   ```

3. **Package for distribution**:
   ```powershell
   dotnet publish --configuration Release
   ```

The compiled plugin will be in `Jellyfin.Plugin.Template/bin/Release/net8.0/publish/`

### Development Setup

For development with automatic deployment to your local Jellyfin instance:

1. **Copy the tasks and settings** from `.vscode/` if using VS Code
2. **Update the paths** in `.vscode/settings.json` to match your environment
3. **Use the build task** to automatically build and deploy to your Jellyfin plugins directory

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

### Development Guidelines

- Follow existing code style and conventions
- Add tests for new functionality
- Update documentation as needed
- Ensure compatibility with Jellyfin 10.9+

## License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built for the [Jellyfin](https://jellyfin.org/) media server
- Based on the [Jellyfin Plugin Template](https://github.com/jellyfin/jellyfin-plugin-template)

## Support

- **Issues**: [GitHub Issues](https://github.com/C2gl/jellyfin-plugin-remind-me/issues)
- **Discussions**: [GitHub Discussions](https://github.com/C2gl/jellyfin-plugin-remind-me/discussions)
- **Jellyfin Community**: [Jellyfin Forums](https://forum.jellyfin.org/)
