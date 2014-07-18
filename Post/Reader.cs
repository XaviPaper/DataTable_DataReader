using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post
{
	public class Reader : IDisposable
	{
		static string connectionString = @"Data Source=(LocalDb)\v11.0;Initial Catalog=Post;Integrated Security=SSPI; MultipleActiveResultSets=True";
		private SqlConnection _connection;

		public async Task<IEnumerable<dynamic>> Read()
		{
			_connection = new SqlConnection(connectionString);
			_connection.Open();

			using (var command = _connection.CreateCommand())
			{
				command.CommandText = "select * from persona";

				return ReadData(await command.ExecuteReaderAsync());
			}
		}
		private IEnumerable<dynamic> ReadData(SqlDataReader dataReader)
		{
			var numColumns = dataReader.FieldCount;
			var values = new object[numColumns];
			var names = new List<string>();

			var firstTime = true;
			while (dataReader.ReadData())
			{
				var record = new Dictionary<string, object>();

				if (dataReader.GetValues(values) > 0)
				{
					for (int i = 0; i < numColumns; i++)
					{
						if (firstTime)
							names.Add(dataReader.GetName(i));

						var value = values[i];
						record.Add(names[i], value != System.DBNull.Value ? value : null);
					}
				}
				firstTime = false;

				yield return record;
			}
		}
		public void Dispose()
		{
			if (_connection != null)
			{
				_connection.Close();
				_connection.Dispose();
				_connection = null;
			}
		}
	}
	public static class SqlDataReaderExtension
	{
		public static bool ReadData(this SqlDataReader that)
		{
			//var task = that.ReadAsync();
			//task.Wait();
			//return task.Result;
			return that.Read();
		}
	}
}
