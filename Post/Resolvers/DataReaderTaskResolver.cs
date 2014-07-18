using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Post.Resolvers
{
	public class DataReaderTaskResolver : IResolver
	{
		public async Task<long> Execute(string connectionString)
		{
			var stopwath = new Stopwatch();
			stopwath.Reset();
			stopwath.Start();

			using (var reader = new Reader())
			{
				var result = await reader.Read();
				var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
			}

			stopwath.Stop();
			return stopwath.ElapsedMilliseconds;
		}
	}
}
