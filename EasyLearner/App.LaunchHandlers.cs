namespace EasyLearner
{
	using Microsoft.AppCenter.Crashes;
	using Microsoft.Extensions.DependencyInjection;
	using Serilog;
	using SysHelpers;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.IO.Pipes;
	using System.Linq;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using Utils;
	using Utils.Helpers;
	using WinUIEx;

	public partial class App
	{
		private const string PipeName = "VIEasyLearner";
		private static Mutex Mutex = null;
		private static NamedPipeServerStream pipeServer;

		public static void TryToRestoreMainWindow()
		{
			DispatcherQueueHelper.EnqueueAsync(async () =>
			{
				if (MainWindow != null)
				{
					MainWindow.WindowState = WindowState.Normal;
					await Task.Delay(1000);
					if (MainWindow != null)
					{
						PinvokeWindowMethods.SetForegroundWindow(MainWindow.GetWindowHandle());
					}
				}
				else
				{
					CreateMainWindow();
					await Task.Delay(1000);
					if (MainWindow != null)
					{
						PinvokeWindowMethods.SetForegroundWindow(MainWindow.GetWindowHandle());
					}
				}
			});
		}

		private static void SendCommandToMainInstance(string command)
		{
			using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", PipeName, PipeDirection.Out))
			{
				try
				{
					pipeClient.Connect(500); // 500 ms timeout

					using (StreamWriter writer = new StreamWriter(pipeClient, Encoding.UTF8))
					{
						writer.Write(command);
					}
				}
				catch (Exception ex)
				{
					Crashes.TrackError(ex);
					Log.Logger.Error(ex, ex.Message);
				}
			}
		}

		private bool CheckSecondStart(string[] args)
		{
			Mutex = new Mutex(true, "VIEasyLearnerMutex", out var createdNew);

			if (!createdNew)
			{
				string msg = "RestoreMainWindow";

				SendCommandToMainInstance(msg);
				Environment.Exit(0);
				return false;
			}
			else
			{
				SetupNamedPipeServer();
			}

			return true;
		}

	 

		private async void HandleClientConnection(IAsyncResult result)
		{
			try
			{
				Log.Logger.Debug("Pipe server connection established");
				pipeServer.EndWaitForConnection(result);

				using (StreamReader reader = new StreamReader(pipeServer))
				{
					string message = await reader.ReadToEndAsync();
					Log.Logger.Debug(message);

					if (message == "RestoreMainWindow")
					{
						TryToRestoreMainWindow();
						return;
					}
				}
			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
			}
			finally
			{
				// Continue to wait for the next connection
				SetupNamedPipeServer();
			}
		}

 

		private async Task InitWithCommandLineParameters(string[] args)
		{
			List<string> commandLineParams = new List<string>();
			for (var index = 1; index < args.Length; index++)
			{
				var value = args[index];
				if (string.IsNullOrWhiteSpace(value) == false)
				{
					Log.Logger.Debug($"Got file {value}");
					commandLineParams.Add(value);
				}
			}
		}

		private void SetupNamedPipeServer()
		{
			pipeServer = new NamedPipeServerStream(PipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Message,
				PipeOptions.Asynchronous);
			pipeServer.BeginWaitForConnection(HandleClientConnection, null);
			Log.Logger.Debug("Pipe server launched");
		}
	}
}
