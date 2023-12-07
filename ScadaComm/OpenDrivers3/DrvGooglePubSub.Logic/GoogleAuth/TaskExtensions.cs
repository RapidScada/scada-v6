using System.Threading;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvGooglePubSub.Logic.GoogleAuth
{
    // Note: this is duplicated between Google.Apis.Auth and Google.Apis.Core so it can stay internal. Please
    // change both at the same time.
    internal static class TaskExtensions
    {
        /// <summary>
        /// Returns a task which can be cancelled by the given cancellation token, but otherwise observes the original
        /// task's state. This does *not* cancel any work that the original task was doing, and should be used carefully.
        /// </summary>
        internal static Task<T> WithCancellationToken<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            if (!cancellationToken.CanBeCanceled)
            {
                return task;
            }

            return ImplAsync();

            // Separate async method to allow the above optimization to avoid creating any new state machines etc.
            async Task<T> ImplAsync()
            {
                var cts = new TaskCompletionSource<T>();
                using (cancellationToken.Register(() => cts.TrySetCanceled()))
                {
                    var completedTask = await Task.WhenAny(task, cts.Task).ConfigureAwait(false);
                    return await completedTask.ConfigureAwait(false);
                }
            }
        }
    }
}
