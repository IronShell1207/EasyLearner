using EasyLearner.Helpers;
using WinUIEx;

namespace EasyLearner
{
	using EasyLearner.Views.Pages;
	using Microsoft.UI.Xaml;

	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : WindowEx
	{
		public MainWindow()
		{
			this.InitializeComponent();
			ExtendsContentIntoTitleBar = true;
			SetTitleBar(AppTitleBar);
			DialogHelper.SetDialogContainer(DialogsPresenter);
		}

		public void Initialize()
		{
			AppLoadingGrid.Visibility = Visibility.Collapsed;
			_rootFrame.Navigate(typeof(MainPage));
		}
	}
}