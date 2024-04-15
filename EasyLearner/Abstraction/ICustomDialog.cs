using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace EasyLearner.Abstraction
{
	using System.Threading.Tasks;

	public interface ICustomDialog
	{
		/// <summary>
		/// Событие завершания работы с диалогом.
		/// </summary>
		public TaskCompletionSource<ContentDialogResult> DialogTaskCompletionResult { get; set; }

		/// <summary>
		/// Контент диалога.
		/// </summary>
		public FrameworkElement Content { get; set; }
	}
}
