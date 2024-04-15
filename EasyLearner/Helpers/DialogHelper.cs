using EasyLearner.Abstraction;
using Serilog;

namespace EasyLearner.Helpers
{
	using Microsoft.UI.Xaml;
	using Microsoft.UI.Xaml.Controls;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class DialogHelper
	{
		private static Grid _dialogContainerGrid;

		/// <summary>
		/// Presenter для вывода диалогов.
		/// </summary>
		private static ContentPresenter _dialogPlacementSource;

		/// <summary>
		/// Показывается ли диалог.
		/// </summary>
		public static bool IsDialogShowing { get; private set; } = false;
		/// <summary>
		/// Показывает кастомный диалог.
		/// </summary>
		public static async Task<ContentDialogResult> ShowCustomDialog(this ICustomDialog dialogModel,
			Func<Task> afterLoadFunc = null, bool showAsWindow = false)
		{
			try
			{
				_dialogContainerGrid.Children.Add(dialogModel.Content);
				_dialogPlacementSource.Visibility = Visibility.Visible;
				if (afterLoadFunc != null)
				{
					await afterLoadFunc.Invoke();
				}

				var result = await dialogModel.DialogTaskCompletionResult.Task;
				_dialogContainerGrid.Children.Remove(dialogModel.Content);

				return result;
			}
			catch (Exception ex)
			{
				Log.Logger.Error(ex, ex.Message);
			}
			finally
			{
				if (_dialogContainerGrid.Children.Any() == false)
				{
					_dialogPlacementSource.Visibility = Visibility.Collapsed;
				}
			}

			return ContentDialogResult.None;
		}

		public static void SetDialogContainer(ContentPresenter presenter)
		{
			_dialogPlacementSource = presenter;
			_dialogContainerGrid = new Grid();
			_dialogPlacementSource.Content = _dialogContainerGrid;
		}
	}
}
