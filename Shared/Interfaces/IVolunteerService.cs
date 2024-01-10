using festival.Shared.Models;
namespace festival.Server.Interfaces
{
    public interface IVolunteerService
    {
        Task<List<Volunteer>> GetAllAsync();
        Task<Volunteer> GetByIdAsync(string id);
        Task<Volunteer> CreateAsync(Volunteer volunteer);
        Task UpdateAsync(string id, Volunteer volunteerIn);
        Task DeleteAsync(string id);

    }
}
    