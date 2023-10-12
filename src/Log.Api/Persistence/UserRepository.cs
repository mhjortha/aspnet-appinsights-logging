using Log.Api.Contracts;
using Log.Api.Data;
using Log.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Log.Api.Persistence;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    

    public async Task AddAsync(User user)
    {
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        IQueryable<User> query = _context.Users!
            .Where(u => u.Id == id);

        return await query.FirstOrDefaultAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllAsync()
    {
        IQueryable<User> query = _context.Users!;

        return await query.ToListAsync();
    }
}