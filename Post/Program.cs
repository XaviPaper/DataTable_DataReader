using Post.Resolvers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post
{
	class Program
	{
		static string connectionString = @"Data Source=(LocalDb)\v11.0;Initial Catalog=Post;Integrated Security=SSPI; MultipleActiveResultSets=True";

		public async static Task Execute()
		{
			int iteraciones = 10;

			{
				var resolver = new DataTableResolver();
				Console.WriteLine("DataTable 1: " + await resolver.Execute(connectionString));
				long result = 0;
				for (var i = 0; i < iteraciones; i++)
					result += await resolver.Execute(connectionString);
				Console.WriteLine("DataTable m: " + result / iteraciones);
			}

			{
				var resolver = new DataReaderResolver();
				Console.WriteLine("DataReader 1: " + await resolver.Execute(connectionString));
				long result = 0;
				for (var i = 0; i < iteraciones; i++)
					result += await resolver.Execute(connectionString);
				Console.WriteLine("DataReader m: " + result / iteraciones);
			}

			{
				var resolver = new DataReaderTaskResolver();
				Console.WriteLine("DataReaderTask 1: " + await resolver.Execute(connectionString));
				long result = 0;
				for (var i = 0; i < iteraciones; i++)
					result += await resolver.Execute(connectionString);
				Console.WriteLine("DataReaderTask m: " + result / iteraciones);
			}

			{
				var resolver = new DataReaderDirectResolver();
				Console.WriteLine("DataReaderDirect 1: " + await resolver.Execute(connectionString));
				long result = 0;
				for (var i = 0; i < iteraciones; i++)
					result += await resolver.Execute(connectionString);
				Console.WriteLine("DataReaderDirect m: " + result / iteraciones);
			}
		}
		static void Main(string[] args)
		{
			Execute().Wait();

			Console.ReadLine();
		}
	}
}
