using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Post.Resolvers
{
	// http://weblog.west-wind.com/posts/2009/Apr/24/JSON-Serialization-of-a-DataReader
	public class DataReaderDirectResolver : IResolver
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
					var stringBuilder = new StringBuilder();
					WriteValue(stringBuilder, dataReader);
				}
			}
			stopwath.Stop();
			return stopwath.ElapsedMilliseconds;				
		}
		private void WriteValue(StringBuilder stringBuilder, object val)
		{

			if (val is IDataReader)
				WriteDataReader(stringBuilder, val as IDataReader);
			//else if (val is IDictionary)
			//	WriteDictionary(sb, val as IDictionary);
			//else if (val is IEnumerable)
			//	WriteEnumerable(sb, val as IEnumerable);
			else if (val is DateTime)
				stringBuilder.Append(((DateTime) val).ToString("o"));
			else
				stringBuilder.Append(val);
		}

		private void WriteDataReader(StringBuilder stringBuilder, IDataReader dataReader)
		{
			var numColumns = dataReader.FieldCount;
			var names = new List<string>();

			var rowCount = 0;
			stringBuilder.Append("{[");
			while (dataReader.Read())
			{
				stringBuilder.Append("{");

				for (int i = 0; i < numColumns; i++)
				{
					if (rowCount == 0)
						names.Add(dataReader.GetName(i));

					stringBuilder.AppendFormat("\"{0}\":", names[i]);
					this.WriteValue(stringBuilder, dataReader[i]);
					stringBuilder.Append(",");
				}

				if (numColumns > 0)
					stringBuilder.Length -= 1;

				stringBuilder.Append("},");
				rowCount++;
			}
			if (rowCount > 0)
				stringBuilder.Length -= 1;
			stringBuilder.Append("]}");
		}
	}
}
