using EasyLearner.Database;
using EasyLearner.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Utils.Helpers;

namespace EasyLearner
{
	using Microsoft.UI.Xaml;
	using Microsoft.UI.Xaml.Controls;
	using Microsoft.UI.Xaml.Controls.Primitives;
	using Microsoft.UI.Xaml.Data;
	using Microsoft.UI.Xaml.Input;
	using Microsoft.UI.Xaml.Media;
	using Microsoft.UI.Xaml.Navigation;
	using Microsoft.UI.Xaml.Shapes;
	using Serilog.Sinks.SystemConsole.Themes;
	using Serilog;
	using SysHelpers;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Runtime.InteropServices.WindowsRuntime;
	using Windows.ApplicationModel;
	using Windows.ApplicationModel.Activation;
	using Windows.Foundation;
	using Windows.Foundation.Collections;
	using System.Threading.Tasks;
	using EasyLearner.ViewModels;

	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public partial class App : Application
	{
		public static IServiceProvider ServiceProvider { get; private set; }
		public static MainWindow MainWindow { get; private set; }

		private static bool _servicesInitialized = false;

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			this.InitializeComponent();
			SetupLogger();
			ConfigureServices();
			DispatcherQueueHelper.SetCurrentThread();
		}

		private void SetupLogger()
		{
#if DEBUG
			PinvokeWindowMethods.AllocConsole();
#endif
			Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose()
				.WriteTo.Console(theme: AnsiConsoleTheme.Code)
				.CreateLogger();
		}

		private void ConfigureServices()
		{
			var services = new ServiceCollection();
			services.AddSingleton<MainViewModel>();
			services.AddSingleton<NotifyService>();
			ServiceProvider = services.BuildServiceProvider();
		}

		public async Task InitializeServices()
		{
			if (_servicesInitialized)
				return;
			await DbHelper.Initialize();

			var converterPageVm = ServiceProvider.GetRequiredService<MainViewModel>();
			await converterPageVm.Initialize();
			var notifySrv = ServiceProvider.GetRequiredService<NotifyService>();
			notifySrv.Init();
			_servicesInitialized = true;
		}

		protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
		{
			string[] cmdLaunchArgs = Environment.GetCommandLineArgs();

			if (CheckSecondStart(cmdLaunchArgs) == false)
				return;

			await InitializeServices();
			CreateMainWindow();

			await InitWithCommandLineParameters(cmdLaunchArgs);
		}

		public static void CreateMainWindow()
		{
			MainWindow = new MainWindow();
			MainWindow.Activate();
			if (_servicesInitialized)
			{
				MainWindow.Initialize();
			}
		}
	}
}