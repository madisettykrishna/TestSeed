using System.Threading.Tasks;

namespace SeedApp.Data.Interfaces
{
    public interface IDbSharingProvider
    {
        Task ShareDbViaEmailAsync();
    }
}