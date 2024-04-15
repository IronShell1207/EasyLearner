namespace EasyLearner.Database
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	internal class DbCommands
	{
		/// <summary>
		/// Часть команды по созданию таблицы, если не существует..
		/// </summary>
		private static string _createTableIfNotExists = "CREATE TABLE IF NOT EXISTS";

		/// <summary>
		/// Название таблицы.
		/// </summary>
		public static string StringsTableName => "StringsTable";

		/// <summary>
		/// Команда вставки.
		/// </summary>
		private static string INSERT_STRING = "INSERT INTO";

		/// <summary>
		/// Текст команды, проверяющий наличие таблицы заметок.
		/// </summary>
		public static string CheckTableExistsCommandText(string tableName) =>
			$"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}' COLLATE NOCASE";

		/// <summary>
		/// Текст команды, создающий таблицу шаблонов.
		/// </summary>
		public static string CreateWallpapersTableCommandText => $@"{_createTableIfNotExists} {StringsTableName}
    (
        id INTEGER PRIMARY key autoincrement NOT NULL,
        strValue VARCHAR,
        translate VARCHAR,
        description BOOLEAN,
        knowTimes INTEGER,
        dontKnowTimes INTEGER,
        learned BOOLEAN
    )";

		/// <summary>
		/// Текст команды по добавлению шаблона.
		/// </summary>
		public static string AddStringCommandText => $@"{INSERT_STRING} {StringsTableName} (strValue, translate, description, knowTimes, dontKnowTimes, learned) VALUES (@p1, @p2, @p3, @p4, @p5, @p6) returning id;";

		/// <summary>
		/// Текст команды по обновлению шаблона.
		/// </summary>
		public static string UpdateStringCommandText(int id) => $@"UPDATE {StringsTableName} SET strValue = @p1, translate = @p2, description = @p3, knowTimes = @p4, dontKnowTimes = @p5, learned = @p6 WHERE id = {id}";

		/// <summary>
		/// Текст команды по удалению шаблона.
		/// </summary>
		public static string DeleteStringByIdCommandText(int id) => $"DELETE FROM {StringsTableName} WHERE id = {id}";

		/// <summary>
		/// Текст команды по получению списка шаблонов.
		/// </summary>
		public static string GetStringsCommandText =>
			$"SELECT id, strValue, translate, description, knowTimes, dontKnowTimes, learned FROM {StringsTableName} ORDER BY id";
	}
}
