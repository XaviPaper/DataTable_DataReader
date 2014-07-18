
using System.Threading.Tasks;
namespace Post
{
	public interface IResolver
	{
		Task<long> Execute(string connectionString);
	}
}
