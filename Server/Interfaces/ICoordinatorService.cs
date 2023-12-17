using festival.Shared.Models;
namespace festival.Server.Interfaces
{
    public interface ICoordinatorService
    {
        Task<List<Coordinator>> GetAllAsync();
        Task<Coordinator> GetByIdAsync(string id);
        Task CreateAsync(Coordinator coordinator);
        Task UpdateAsync(string id, Coordinator coordinatorIn);
        Task DeleteAsync(string id);
    }
}
