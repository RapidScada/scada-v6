using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using Google.Apis.Util;

namespace Scada.Comm.Drivers.DrvGooglePubSub.Logic.GoogleAuth
{
    internal sealed class CloudScadaTokenRefreshManager
    {
        // Immutable state
        private readonly object _lock = new object();
        private readonly IClock _clock;
        private readonly Func<CancellationToken, Task<bool>> _refreshAction;

        // Mutable state, guarded with _lock.
        private CloudScadaTokenResponse _token;
        private Task<CloudScadaTokenResponse> _refreshTask;

        /// <summary>
        /// Creates a manager which executes the given refresh action when required.
        /// </summary>
        /// <param name="refreshAction">The refresh action which will populate the Token property when successful.</param>
        /// <param name="clock">The clock to consult for timeouts.</param>
        /// <param name="logger">The logger to use to record refreshes.</param>
        internal CloudScadaTokenRefreshManager(Func<CancellationToken, Task<bool>> refreshAction, IClock clock)
        {
            _refreshAction = refreshAction;
            _clock = clock;
        }

        internal CloudScadaTokenResponse Token
        {
            get
            {
                lock (_lock)
                {
                    return _token;
                }
            }
            // The token may be set due to operations other than GetAccessTokenForRequestAsync, but we don't need to
            // null out _refreshTask if so.
            set
            {
                lock (_lock)
                {
                    if (value != null) _token = value;
                }
            }
        }

        internal async Task<string> GetAccessTokenForRequestAsync(CancellationToken cancellationToken)
        {
            Task<CloudScadaTokenResponse> refreshTask;
            lock (_lock)
            {
                // If current token is not soft-expired, then return it.
                if (_token != null && !_token.IsExpired(_clock))
                {
                    return _token.AccessToken;
                }
                // Token refresh required, so start a task if not already started
                if (_refreshTask == null)
                {
                    // Task.Run is required if the refresh completes synchronously,
                    // otherwise _refreshTask is updated in an incorrect order.
                    // And Task.Run also means it can be run here in the lock.
                    _refreshTask = Task.Run(RefreshTokenAsync, cancellationToken);

                    // Let's make sure that exceptions in _refreshTask are always observed.
                    // Note that we don't keep a reference to this new task as we don't really
                    // care about the errors, and we want calling code explicitly awaiting on _refreshTask
                    // to actually fail if there's an error. We just schedule it to run and that's enough for
                    // avoiding exception observavility issues.
                    _refreshTask.ContinueWith(LogException, TaskContinuationOptions.OnlyOnFaulted);
                }
                // If current token is not hard-expired, then return it.
                if (_token != null && !_token.IsEffectivelyExpired(_clock))
                {
                    return _token.AccessToken;
                }
                refreshTask = _refreshTask;

                async Task LogException(Task task)
                {
                    try
                    {
                        await task.ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occured on a background token refresh task.{Environment.NewLine}{ex}");
                    }
                }
            }

            refreshTask = refreshTask.WithCancellationToken(cancellationToken);

            // Note that strictly speaking, the token returned here may already be soft, hard or really expired.
            // This may happen for tokens that are short lived enough, or in systems with significant load
            // where maybe the token itself was obtained quickly but the task could not acquire a thread fast enough
            // in which to resume.
            // We don't retry as the conditions under which this may happen are not inmediately recoverable and possibly rare,
            // and the token will be refreshed again on a subsequent token request.
            // Also, note that the token is unusable only if it's really expired, if it's soft/hard expire it still may be valid
            // if used fast enough.
            return (await refreshTask.ConfigureAwait(false)).AccessToken;
        }

        internal static readonly TimeSpan[] RefreshTimeouts = new[] { TimeSpan.FromSeconds(12), TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5) };
        private async Task<CloudScadaTokenResponse> RefreshTokenAsync()
        {
            Console.WriteLine("Token has expired, trying to get a new one.");
            try
            {
                List<string> errors = null;
                foreach (var timeout in RefreshTimeouts)
                {
                    var token = new CancellationTokenSource(timeout).Token;
                    try
                    {
                        var success = await _refreshAction(token).ConfigureAwait(false);
                        if (success)
                        {
                            Console.WriteLine("New access token was received successfully");
                            return Token;
                        }
                        else
                        {
                            // If unsuccessful, but didn't timeout, then retry if all retries haven't been exhausted.
                            (errors = errors ?? new List<string>()).Add("refresh error");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // Do nothing, attempt another refresh if all retries haven't been exhausted.
                        Console.WriteLine("Token refresh time-out after {0} seconds", (int)timeout.TotalSeconds);
                        (errors = errors ?? new List<string>()).Add("timeout");
                    }
                }
                throw new InvalidOperationException($"The access token has expired and could not be refreshed. Errors: {string.Join(", ", errors)}");
            }
            finally
            {
                // If the task completed successfully, Token will have been set.
                // Otherwise, we'll want to start a new refresh task next time we're asked for a token.
                lock (_lock)
                {
                    _refreshTask = null;
                }
            }
        }

        // Helper method used in the continuation when trying to create a cancellable task.
        private static T ResultWithUnwrappedExceptions<T>(Task<T> task)
        {
            try
            {
                task.Wait();
            }
            catch (AggregateException e)
            {
                // Unwrap the first exception, a bit like await would.
                // It's very unlikely that we'd ever see an AggregateException without an inner exceptions,
                // but let's handle it relatively gracefully.
                // Using ExceptionDispatchInfo to keep the original exception stack trace.
                ExceptionDispatchInfo.Capture(e.InnerExceptions.FirstOrDefault() ?? e).Throw();
            }
            return task.Result;
        }
    }
}
