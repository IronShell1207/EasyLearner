using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyLearner.Database;
using System.Collections.ObjectModel;

namespace EasyLearner.ViewModels
{
	using System.Linq;
	using System.Threading.Tasks;

	internal class MainViewModel : ObservableObject
	{
		/// <inheritdoc cref="NewStringValue"/>
		private LearningStringViewModel _newStringValue = new LearningStringViewModel();

		/// <summary>
		/// Инициализирует экземпляр <see cref="MainViewModel"/>.
		/// </summary>
		public MainViewModel()
		{
			AddStringAsyncCommand = new AsyncRelayCommand(OnAddStringAsync);
		}

		/// <summary>
		/// Команда выполняющая метод <see cref="OnAddStringAsync"/>.
		/// </summary>
		public AsyncRelayCommand AddStringAsyncCommand { get; }

		public ObservableCollection<LearningStringViewModel> LearningStrings { get; set; } =
									new ObservableCollection<LearningStringViewModel>();

		/// <summary>
		/// Новое значение.
		/// </summary>
		public LearningStringViewModel NewStringValue {
			get => _newStringValue;
			set => SetProperty(ref _newStringValue, value);
		}

		public async Task Initialize()
		{
			var stringsList = await DbHelper.GetStringsList();

			if (stringsList != null && stringsList.Any())
			{
				foreach (var stringModel in stringsList)
				{
					LearningStrings.Add(stringModel);
				}
			}
		}

		/// <summary>
		/// Добавляет строку
		/// </summary>
		private async Task OnAddStringAsync()
		{
			LearningStrings.Add(NewStringValue);
			await DbHelper.UpdateStringModel(NewStringValue);
			NewStringValue = new LearningStringViewModel();
		}
	}
}