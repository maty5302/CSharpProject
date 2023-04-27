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
			StringBuilder sb = new StringBuilder();
			sb.Append("INSERT INTO ").Append("[").Append(typeof(T).Name).Append("]").AppendLine("(");
			Type type = typeof(T);
			var members = type.GetProperties();
			foreach (var item2 in members)
			{
				if (item2.Name.ToLower().Contains("id"))
					continue;
				sb.Append(item2.Name);
				if (item2 != members.Last())
					sb.AppendLine(",");
			}
			sb.AppendLine(") VALUES (");
			foreach (var item2 in members)
			{
				if (item2.Name.ToLower().Contains("id"))
					continue;
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
				connection.Close();
			}
		}

	}
}
