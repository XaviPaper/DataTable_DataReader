using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Post.Resolvers
{
	public class DataTableResolver : IResolver
	{
		public async Task<long> Execute(string connectionString)
		{
			var stopwath = new Stopwatch();
			stopwath.Reset();
			stopwath.Start();

			var result = new DataTable();
			using (var connection = new SqlConnection(connectionString))
			using (var command = new SqlCommand("select * from persona"))
			using (var dataAdapter = new SqlDataAdapter(command))
			{
				await connection.OpenAsync();
				command.Connection = connection;
				dataAdapter.Fill(result);
			}
			var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);

			stopwath.Stop();
			return stopwath.ElapsedMilliseconds;
		}
	}
}
