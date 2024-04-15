using CommunityToolkit.Mvvm.ComponentModel;

namespace EasyLearner.ViewModels
{
	public class LearningStringViewModel : ObservableObject
	{
		/// <inheritdoc cref="Description"/>
		private string _description = "";

		/// <inheritdoc cref="IsLearned"/>
		private bool _isLearned = false;

		/// <inheritdoc cref="LeanedCounter"/>
		private int _leanedCounter = 0;

		/// <inheritdoc cref="NotLearnedCounter"/>
		private int _notLearnedCounter = 0;

		/// <inheritdoc cref="Translate"/>
		private string _translate = "";

		/// <inheritdoc cref="Value"/>
		private string _value = "";

		/// <summary>
		/// Инициализирует экземпляр <see cref="LearningStringViewModel"/>.
		/// </summary>
		public LearningStringViewModel(string value, string translate, int id)
		{
			_value = value;
			_translate = translate;
			Id = id;
		}

		/// <summary>
		/// Инициализирует экземпляр <see cref="LearningStringViewModel"/>.
		/// </summary>
		public LearningStringViewModel()
		{

		}

		/// <summary>
		/// Описание..
		/// </summary>
		public string Description {
			get => _description;
			set => SetProperty(ref _description, value);
		}

		public int Id { get; set; } = -1;

		/// <summary>
		/// Выучено ли.
		/// </summary>
		public bool IsLearned {
			get => _isLearned;
			set => SetProperty(ref _isLearned, value);
		}

		/// <summary>
		/// Количество удачных отгадок.
		/// </summary>
		public int LeanedCounter {
			get => _leanedCounter;
			set => SetProperty(ref _leanedCounter, value);
		}

		/// <summary>
		/// Количество неудачных отгадок.
		/// </summary>
		public int NotLearnedCounter {
			get => _notLearnedCounter;
			set => SetProperty(ref _notLearnedCounter, value);
		}

		/// <summary>
		/// Перевод.
		/// </summary>
		public string Translate {
			get => _translate;
			set => SetProperty(ref _translate, value);
		}

		/// <summary>
		/// Сама строка.
		/// </summary>
		public string Value {
			get => _value;
			set => SetProperty(ref _value, value);
		}
	}
}