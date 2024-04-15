using System.IO;

namespace EasyLearner.Database
{
	using EasyLearner.ViewModels;
	using Microsoft.Data.Sqlite;
	using Microsoft.VisualBasic;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Windows.Storage;

	public class DbHelper
	{
 

		/// <summary>
		/// Инициализирует таблицу ImageDbModel.
		/// </summary>
		public static async Task Initialize()
		{
			string connectionString =
				"Data source=" + Path.Combine(ApplicationData.Current.LocalFolder.Path, "learingWords.db");
			using var connection = new SqliteConnection(connectionString);
			await connection.OpenAsync();
			var sqlCommand = connection.CreateCommand();
			sqlCommand.CommandText = DbCommands.CheckTableExistsCommandText(DbCommands.StringsTableName);
			var result = await sqlCommand.ExecuteReaderAsync();
			if (result.HasRows == false)
			{
				var createTableCommand = connection.CreateCommand();
				createTableCommand.CommandText = DbCommands.CreateWallpapersTableCommandText;
				await createTableCommand.ExecuteNonQueryAsync();
			}
			connection.Close();
			connection.Dispose();
		}

		/// <summary>
		/// Удаляет ImageDbModel.
		/// </summary>
		public static async Task DeleteString(LearningStringViewModel docModel)
		{
			string connectionString =
				"Data source=" + Path.Combine(ApplicationData.Current.LocalFolder.Path, "learingWords.db");
			bool isIdLoaded = docModel.Id >= 0;
			if (isIdLoaded)
			{
				using var connection = new SqliteConnection(connectionString);
				await connection.OpenAsync();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = DbCommands.DeleteStringByIdCommandText(docModel.Id);
				await sqlCommand.ExecuteNonQueryAsync();
			}
		}

		/// <summary>
		/// Получает список .
		/// </summary>
		public static async Task<List<LearningStringViewModel>> GetStringsList()
		{
			string connectionString =
				"Data source=" + Path.Combine(ApplicationData.Current.LocalFolder.Path, "learingWords.db");
			List<LearningStringViewModel> imagesList = new List<LearningStringViewModel>();

			using var connection = new SqliteConnection(connectionString);
			await connection.OpenAsync();
			var sqlCommand = connection.CreateCommand();
			sqlCommand.CommandText = DbCommands.GetStringsCommandText;
			var resultReader = await sqlCommand.ExecuteReaderAsync();

			while (await resultReader.ReadAsync())
			{
				var imageModel = new LearningStringViewModel()
				{
					Id = resultReader.TryGetInt32(0),
					Value = resultReader.TryGetString(1),
					Translate = resultReader.TryGetString(2),
					Description = resultReader.GetString(3),
					LeanedCounter = resultReader.GetInt32(4),
					NotLearnedCounter = resultReader.GetInt32(5),
					IsLearned = resultReader.GetBoolean(6)
				};
				imagesList.Add(imageModel);
			}

			return imagesList;
		}

		/// <summary>
		/// Обновляет ImageDbModel.
		/// </summary>
		public static async Task UpdateStringModel(LearningStringViewModel image)
		{
			string connectionString =
				"Data source=" + Path.Combine(ApplicationData.Current.LocalFolder.Path, "learingWords.db");
			if (image.Id < 0)
			{
				await InsertStringModel(image);
				return;
			}
			using var connection = new SqliteConnection(connectionString);
			await connection.OpenAsync();
			var sqlCommand = connection.CreateCommand();
			sqlCommand.CommandText = DbCommands.UpdateStringCommandText(image.Id);
			sqlCommand.Parameters.AddWithValue("@p1", image.Value);
			sqlCommand.Parameters.AddWithValue("@p2", image.Translate);
			sqlCommand.Parameters.AddWithValue("@p3", image.Description);
			sqlCommand.Parameters.AddWithValue("@p4", image.LeanedCounter);
			sqlCommand.Parameters.AddWithValue("@p5", image.NotLearnedCounter);
			sqlCommand.Parameters.AddWithValue("@p6", image.IsLearned);
			await sqlCommand.ExecuteNonQueryAsync();
		}

		/// <summary>
		/// Вставляет ImageDbModel.
		/// </summary>
		public static async Task InsertStringModel(LearningStringViewModel image)
		{
			string connectionString =
				"Data source=" + Path.Combine(ApplicationData.Current.LocalFolder.Path, "learingWords.db");
			using var connection = new SqliteConnection(connectionString);
			await connection.OpenAsync();
			var sqlCommand = connection.CreateCommand();
			sqlCommand.CommandText = DbCommands.AddStringCommandText;
			sqlCommand.Parameters.AddWithValue("@p1", image.Value);
			sqlCommand.Parameters.AddWithValue("@p2", image.Translate);
			sqlCommand.Parameters.AddWithValue("@p3", image.Description);
			sqlCommand.Parameters.AddWithValue("@p4", image.LeanedCounter);
			sqlCommand.Parameters.AddWithValue("@p5", image.NotLearnedCounter);
			sqlCommand.Parameters.AddWithValue("@p6", image.IsLearned);
			var resultReader = await sqlCommand.ExecuteReaderAsync();
			while (await resultReader.ReadAsync())
			{
				image.Id = resultReader.TryGetInt32(0);
			}
		}
	}
}
