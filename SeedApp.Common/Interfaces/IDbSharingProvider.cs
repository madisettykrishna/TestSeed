using System.Threading.Tasks;

namespace SeedApp.Common.Interfaces
{
    public interface IDbSharingProvider
    {
        Task ShareDbViaEmailAsync();
    }
}