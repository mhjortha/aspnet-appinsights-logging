using Log.Api.Contracts;
using Log.Api.Models;

namespace Log.Api.Persistence;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User> GetByIdAsync(int id);
    Task DeleteAsync(User user);
    Task<List<User>> GetAllAsync();
}