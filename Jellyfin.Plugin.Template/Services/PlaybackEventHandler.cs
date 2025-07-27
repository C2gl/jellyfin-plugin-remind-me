using System;
using System.Linq;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Plugins;
using MediaBrowser.Controller.Session;
using MediaBrowser.Model.Entities;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Template.Services;

/// <summary>
/// Handles playback events to queue next movies in series.
/// </summary>
public class PlaybackEventHandler : IServerEntryPoint
{
    private readonly ISessionManager _sessionManager;
    private readonly ILibraryManager _libraryManager;
    private readonly IUserDataManager _userDataManager;
    private readonly ILogger<PlaybackEventHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaybackEventHandler"/> class.
    /// </summary>
    /// <param name="sessionManager">Instance of the <see cref="ISessionManager"/> interface.</param>
    /// <param name="libraryManager">Instance of the <see cref="ILibraryManager"/> interface.</param>
    /// <param name="userDataManager">Instance of the <see cref="IUserDataManager"/> interface.</param>
    /// <param name="logger">Instance of the <see cref="ILogger{PlaybackEventHandler}"/> interface.</param>
    public PlaybackEventHandler(
        ISessionManager sessionManager,
        ILibraryManager libraryManager,
        IUserDataManager userDataManager,
        ILogger<PlaybackEventHandler> logger)
    {
        _sessionManager = sessionManager;
        _libraryManager = libraryManager;
        _userDataManager = userDataManager;
        _logger = logger;
    }

    /// <inheritdoc />
    public Task RunAsync()
    {
        _sessionManager.PlaybackStopped += OnPlaybackStopped;
        _logger.LogInformation("Next in Series playback event handler started");
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _sessionManager.PlaybackStopped -= OnPlaybackStopped;
    }

    private async void OnPlaybackStopped(object? sender, PlaybackStopEventArgs e)
    {
        try
        {
            // Only handle movies
            if (e.Item is not Movie movie)
            {
                return;
            }

            var config = Plugin.Instance?.Configuration;
            if (config == null || !config.EnableAutoQueue)
            {
                return;
            }

            // Check if user watched enough of the movie
            var userData = _userDataManager.GetUserData(e.Session.UserId, movie);
            var watchedPercentage = userData.PlayedPercentage ?? 0;
            
            if (watchedPercentage < config.RequireFullWatchPercentage)
            {
                _logger.LogDebug("Movie {MovieName} only watched {Percentage}%, not queuing next", 
                    movie.Name, watchedPercentage);
                return;
            }

            _logger.LogInformation("Movie {MovieName} was fully watched, looking for next in series", movie.Name);

            // Find the next movie in any collection this movie belongs to
            var nextMovie = await FindNextMovieInSeries(movie, e.Session.UserId);
            
            if (nextMovie != null)
            {
                await QueueNextMovie(nextMovie, e.Session.UserId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling playback stopped event");
        }
    }

    private async Task<Movie?> FindNextMovieInSeries(Movie currentMovie, Guid userId)
    {
        try
        {
            // Get all collections this movie belongs to
            var collections = _libraryManager.GetItemList(new InternalItemsQuery
            {
                IncludeItemTypes = new[] { nameof(BoxSet) },
                Recursive = true
            }).OfType<BoxSet>()
            .Where(collection => collection.Children.Any(child => child.Id == currentMovie.Id))
            .ToList();

            foreach (var collection in collections)
            {
                _logger.LogDebug("Checking collection: {CollectionName}", collection.Name);
                
                // Get all movies in this collection, ordered by sort name or release date
                var moviesInCollection = collection.Children
                    .OfType<Movie>()
                    .OrderBy(m => m.SortName ?? m.Name)
                    .ThenBy(m => m.PremiereDate ?? DateTime.MinValue)
                    .ToList();

                // Find current movie index
                var currentIndex = moviesInCollection.FindIndex(m => m.Id == currentMovie.Id);
                
                if (currentIndex >= 0 && currentIndex < moviesInCollection.Count - 1)
                {
                    var nextMovie = moviesInCollection[currentIndex + 1];
                    
                    // Check if next movie is already watched
                    var userData = _userDataManager.GetUserData(userId, nextMovie);
                    if (!userData.Played)
                    {
                        _logger.LogInformation("Found next movie in collection {CollectionName}: {NextMovie}", 
                            collection.Name, nextMovie.Name);
                        return nextMovie;
                    }
                }
            }

            _logger.LogDebug("No next unwatched movie found in any collection for {MovieName}", currentMovie.Name);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding next movie in series for {MovieName}", currentMovie.Name);
            return null;
        }
    }

    private async Task QueueNextMovie(Movie nextMovie, Guid userId)
    {
        try
        {
            // Mark as "started" with 0% progress to add to continue watching
            var userData = _userDataManager.GetUserData(userId, nextMovie);
            userData.PlaybackPositionTicks = 1; // Just 1 tick to mark as "started"
            userData.LastPlayedDate = DateTime.UtcNow;
            
            _userDataManager.SaveUserData(userId, nextMovie, userData, UserDataSaveReason.UpdateUserData, default);
            
            _logger.LogInformation("Added {MovieName} to continue watching for user", nextMovie.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error queuing next movie {MovieName}", nextMovie.Name);
        }
    }
}
