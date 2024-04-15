using Microsoft.Data.Sqlite;

namespace EasyLearner.Database
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	internal static class DbEx
	{
		/// <summary>
		/// Пытается получить строку по расположению.
		/// </summary>
		public static string TryGetString(this SqliteDataReader reader, int id)
		{
			try
			{
				return reader.GetString(id);
			}
			catch (System.Exception ex)
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Пытается получить значение по расположению.
		/// </summary>
		public static int TryGetInt32(this SqliteDataReader reader, int id)
		{
			try
			{
				return reader.GetInt32(id);
			}
			catch (System.Exception ex)
			{
				return -1;
			}
		}
	}
}
