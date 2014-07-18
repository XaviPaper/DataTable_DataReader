using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Post.Resolvers
{
	public class DataReaderResolver : IResolver
	{
		public async Task<long> Execute(string connectionString)
		{
			var stopwath = new Stopwatch();
			stopwath.Reset();
			stopwath.Start();

			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					command.CommandText = "select * from persona";

					var dataReader = await command.ExecuteReaderAsync();
					var numColumns = dataReader.FieldCount;
					var values = new object[numColumns];
					var names = new List<string>();

					var firstTime = true;
					var result = new List<Dictionary<string, object>>();
					while (dataReader.Read())
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
							firstTime = false;
						}
						result.Add(record);
					}
					var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
				}
			}

			stopwath.Stop();
			return stopwath.ElapsedMilliseconds;
		}
	}
}
