using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using Microsoft.Data.Sqlite;

namespace DataLayer
{
	public class DB
	{
		private static string FindSolutionDirectory(string currentDirectory)
		{
			string[] files = Directory.GetFiles(currentDirectory, "*.sln");

			if (files.Length > 0)
			{
				return currentDirectory;
			}
			else
			{
				string parentDirectory = Directory.GetParent(currentDirectory)?.FullName;

				if (parentDirectory == null)
				{
					return null;
				}
				else
				{
					return FindSolutionDirectory(parentDirectory);
				}
			}
		}
		public static string solutionDirectory = FindSolutionDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
		public static string _connectionString = "Data Source=" + solutionDirectory + "\\restaurace.db";

		private static string getSqlType(PropertyInfo property)
		{
			switch(property.PropertyType.Name)
			{
				case "Int32":
					return "INTEGER";
				case "String":
					return "TEXT";
				case "Boolean":
					return "INTEGER";;
				case "DateTime":
					return "TEXT";
				default:
					if(property.PropertyType.IsClass)
					{
						return "INTEGER";
					}
					throw new Exception("Neznam pipiku");
			}
		}

		public static void CreateTable<T>()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("CREATE TABLE IF NOT EXISTS ").Append("[").Append(typeof(T).Name).Append("]").AppendLine("(");
			Console.WriteLine(typeof(T).Name);
			Type type = typeof(T);
			var members = type.GetProperties();
			foreach (var item in members)
			{
				if(item.Name.ToLower().Contains("id"))
					sb.Append(item.Name).Append(" ").Append(getSqlType(item)).Append(" PRIMARY KEY");
				else
				{
					if (item.PropertyType.IsClass && getSqlType(item)=="INTEGER")
					{
						sb.Append(item.Name).Append("Id ").Append(getSqlType(item));
						sb.Append(" REFERENCES ").Append(item.PropertyType.Name).Append("(Id)");
					}
					else
						sb.Append(item.Name).Append(" ").Append(getSqlType(item));
				}


				if(item!=members.Last())
					sb.AppendLine(",");

			}

			sb.AppendLine(");");

			Console.WriteLine(sb);
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
			{
				connection.Open();
				using (SqliteCommand command = connection.CreateCommand())
				{
					command.CommandText = sb.ToString();
					command.ExecuteNonQuery();
				}
				connection.Close();
			}
		}

		public static void Insert<T>(T item)
		{
			if (item == null)
				throw new Exception("Item is null");
			StringBuilder sb = new StringBuilder();
			sb.Append("INSERT INTO ").Append("[").Append(typeof(T).Name).Append("]").AppendLine("(");
			Type type = typeof(T);
			var members = type.GetProperties();
			foreach (var item2 in members)
			{
				if (item2.Name.ToLower().Contains("id"))
					continue;
				else if (item2.PropertyType.IsClass && getSqlType(item2) == "INTEGER")
					sb.Append(item2.Name).Append("Id");
				else
					sb.Append(item2.Name);
				if (item2 != members.Last())
					sb.AppendLine(",");
			}
			sb.AppendLine(") VALUES (");
			foreach (var item2 in members)
			{
				if (item2.Name.ToLower().Contains("id"))
					continue;
				else if(item2.PropertyType.IsClass && getSqlType(item2) == "INTEGER")
					sb.Append("'").Append(item2.GetValue(item).GetType().GetProperty("Id").GetValue(item2.GetValue(item))).Append("'");
				else
					sb.Append("'").Append(item2.GetValue(item)).Append("'");
				if (item2 != members.Last())
					sb.AppendLine(",");
			}
			sb.AppendLine(");");
			Console.WriteLine(sb);
			using (SqliteConnection connection = new SqliteConnection(_connectionString))
			{
				connection.Open();
				using (SqliteCommand command = connection.CreateCommand())
				{
					command.CommandText = sb.ToString();
					command.ExecuteNonQuery();
				}

				using (SqliteCommand cmd = connection.CreateCommand())
				{
					cmd.CommandText = "SELECT last_insert_rowid()";
					using (SqliteDataReader reader = cmd.ExecuteReader())
					{
						reader.Read();
						item.GetType().GetProperty("Id").SetValue(item, reader.GetInt32(0));
					}
				}
				connection.Close();
			}
		}

		public static void Update<T>(T item)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("UPDATE ").Append("[").Append(typeof(T).Name).Append("]").AppendLine(" SET ");
			Type type = typeof(T);
			var members = type.GetProperties();
			foreach (var item2 in members)
			{
				if (item2.Name.ToLower().Contains("id"))
					continue;
				else if (item2.PropertyType.IsClass && getSqlType(item2) == "INTEGER")
					sb.Append(item2.Name+"Id").Append(" = '").Append(item2.GetValue(item).GetType().GetProperty("Id").GetValue(item2.GetValue(item))).Append("'");
				else
					sb.Append(item2.Name).Append(" = '").Append(item2.GetValue(item)).Append("'");
				if (item2 != members.Last())
					sb.AppendLine(",");
			}
			sb.Append(" WHERE Id = ").AppendLine(item.GetType().GetProperty("Id").GetValue(item).ToString());
			Console.WriteLine(sb);
			using (SqliteConnection connection = new SqliteConnection(_connectionString))
			{
				connection.Open();
				using (SqliteCommand command = connection.CreateCommand())
				{
					command.CommandText = sb.ToString();
					command.ExecuteNonQuery();
				}
				connection.Close();
			}
		}

		public static void Delete<T>(T item)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("DELETE FROM ").Append("[").Append(typeof(T).Name).Append("]").AppendLine(" WHERE Id = ").AppendLine(item.GetType().GetProperty("Id").GetValue(item).ToString());
			Console.WriteLine(sb);
			using (SqliteConnection connection = new SqliteConnection(_connectionString))
			{
				connection.Open();
				using (SqliteCommand command = connection.CreateCommand())
				{
					command.CommandText = sb.ToString();
					int rowsAffected = command.ExecuteNonQuery();
					if (rowsAffected == 0)
						throw new Exception("No rows affected");
				}
				connection.Close();
			}
		}

		public static List<T> SelectAll<T>()
		{ 
			StringBuilder sb = new StringBuilder();
			sb.Append("SELECT * FROM ").Append("[").Append(typeof(T).Name).Append("]");
			Console.WriteLine(sb);
			List<T> list = new List<T>();
			using (SqliteConnection connection = new SqliteConnection(_connectionString))
			{
				connection.Open();
				using (SqliteCommand command = connection.CreateCommand())
				{
					command.CommandText = sb.ToString();
					using (SqliteDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							T item = Activator.CreateInstance<T>();
							Type type = typeof(T);
							var members = type.GetProperties();
							foreach (var item2 in members)
							{
								if (item2.Name.ToLower().Contains("id"))
									item2.SetValue(item, reader.GetInt32(reader.GetOrdinal(item2.Name)));
								else if (item2.PropertyType == typeof(bool))
									item2.SetValue(item, reader.GetBoolean(reader.GetOrdinal(item2.Name)));
								else if(item2.PropertyType==typeof(DateTime))
								{
									string date = reader.GetString(reader.GetOrdinal(item2.Name));
									if (date != null)
										item2.SetValue(item, DateTime.Parse(date));
								}
								else if(item2.PropertyType==typeof(Int32))
								{
									var idk = reader.GetValue(reader.GetOrdinal(item2.Name));
									if (idk != null)
										item2.SetValue(item, Convert.ToInt32(idk));
								}
								else if (item2.PropertyType.IsClass && getSqlType(item2) == "INTEGER")
								{
									var idk = reader.GetValue(reader.GetOrdinal(item2.Name+"Id"));
									var item3 = Activator.CreateInstance(item2.PropertyType);
									if(item2.PropertyType == typeof(User))
										item3 = SelectById<User>(Convert.ToInt32(idk));
									else if(item2.PropertyType == typeof(RTable))
										item3 = SelectById<RTable>(Convert.ToInt32(idk));

									item2.SetValue(item, item3);
								}
								else
									item2.SetValue(item, reader.GetValue(reader.GetOrdinal(item2.Name)));
							}
							list.Add(item);
						}
					}
				}
				connection.Close();
			}
			return list;
		}
		
		

		public static T SelectById<T>(int id)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("SELECT * FROM ").Append("[").Append(typeof(T).Name).Append("] WHERE Id=").Append(id);
			Console.WriteLine(sb);
			T item = Activator.CreateInstance<T>();
			using (SqliteConnection connection = new SqliteConnection(_connectionString))
			{
				connection.Open();
				using (SqliteCommand command = connection.CreateCommand())
				{
					command.CommandText = sb.ToString();
					using (SqliteDataReader reader = command.ExecuteReader())
					{
						reader.Read();
						Type type = typeof(T);
						var members = type.GetProperties();
						foreach (var item2 in members)
						{
							if (item2.Name.ToLower().Contains("id"))
								item2.SetValue(item, reader.GetInt32(reader.GetOrdinal(item2.Name)));
							else if (item2.PropertyType == typeof(bool))
								item2.SetValue(item, reader.GetBoolean(reader.GetOrdinal(item2.Name)));
							else if (item2.PropertyType == typeof(DateTime))
							{
								string date = reader.GetString(reader.GetOrdinal(item2.Name));
								if (date != null)
									item2.SetValue(item, DateTime.Parse(date));
							}
							else if (item2.PropertyType == typeof(Int32))
							{
								var idk = reader.GetValue(reader.GetOrdinal(item2.Name));
								if (idk != null)
									item2.SetValue(item, Convert.ToInt32(idk));
							}
							else if (item2.PropertyType.IsClass && getSqlType(item2) == "INTEGER")
							{
								var idk = reader.GetValue(reader.GetOrdinal(item2.Name + "Id"));
								var item3 = Activator.CreateInstance(item2.PropertyType);
								if (item2.PropertyType == typeof(User))
									item3 = SelectById<User>(Convert.ToInt32(idk));
								else if (item2.PropertyType == typeof(RTable))
									item3 = SelectById<RTable>(Convert.ToInt32(idk));

								item2.SetValue(item, item3);
							}
							else
								item2.SetValue(item, reader.GetValue(reader.GetOrdinal(item2.Name)));
						}
					}
				}
				connection.Close();
			}
			return item;
		}

	
	}
}
