using festival.Shared.Models;
namespace festival.Server.Interfaces
{
    public interface IShiftService
    {
        Task<List<Shift>> GetAllAsync();
        Task<Shift> GetByIdAsync(string id);
        Task CreateAsync(Shift shift);
        Task UpdateAsync(string id, Shift shiftIn);
        Task DeleteAsync(string id);
    }
}
