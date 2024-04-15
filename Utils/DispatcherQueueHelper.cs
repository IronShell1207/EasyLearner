using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;

namespace Utils.Helpers;

#if !WPF

using System;
using System.Threading.Tasks;

#if NETFX_CORE
using Windows.System;
using Microsoft.Toolkit.Uwp;
#endif

#if WIN_APP_SDK
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
#endif


public static class DispatcherQueueHelper
{

	private static DispatcherQueue _dispatcherQueue;

	private static string DispatcherExceptionString => $"DispatcherQueue not initialized, add {nameof(SetCurrentThread)} to app constructor";


	public static DispatcherQueueTimer CreateTimer(TimeSpan interval)
	{
		try
		{
			DispatcherQueueTimer timer = _dispatcherQueue.CreateTimer();
			timer.Interval = interval;

			return timer;
		}
		catch (NullReferenceException)
		{
			throw new NullReferenceException(DispatcherExceptionString);
		}
	}

	public static async Task EnqueueAsync(Action function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
	{
		try
		{
			await _dispatcherQueue.EnqueueAsync(function, priority);
		}
		catch (NullReferenceException)
		{
			throw new NullReferenceException(DispatcherExceptionString);
		}
	}

	public static async Task<T> EnqueueAsync<T>(Func<T> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
	{
		try
		{
			return await _dispatcherQueue.EnqueueAsync(function, priority);
		}
		catch (NullReferenceException)
		{
			throw new NullReferenceException(DispatcherExceptionString);
		}
	}

	public static async Task EnqueueAsync(Func<Task> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
	{
		try
		{
			await _dispatcherQueue.EnqueueAsync(function, priority);
		}
		catch (NullReferenceException)
		{
			throw new NullReferenceException(DispatcherExceptionString);
		}
	}

	public static async Task<T> EnqueueAsync<T>(Func<Task<T>> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
	{
		try
		{
			return await _dispatcherQueue.EnqueueAsync(function, priority);
		}
		catch (NullReferenceException)
		{
			throw new NullReferenceException(DispatcherExceptionString);
		}
	}

	public static void SetCurrentThread() => _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
}

#endif
