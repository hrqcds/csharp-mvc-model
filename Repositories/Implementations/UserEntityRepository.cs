using Database;
using Microsoft.EntityFrameworkCore;
using Models;
using Utils;

namespace Repositories.Implementations;

public class UserEntityRepository : IUserRepository
{

    private readonly Context context;

    public UserEntityRepository(Context _context)
    {
        context = _context;
    }

    public async Task<User?> Create(User user)
    {
        var u = context.Users.Add(user);
        await context.SaveChangesAsync();

        return u.Entity;
    }


    public async Task<List<User>> GetALL(UserQueryRequest query)
    {
        return await context.Users
            .Where(u => u.Name.Contains(query.Name ?? ""))
            .Where(u => u.Email.Contains(query.Email ?? ""))
            .Where(u => u.Register.Contains(query.Register ?? ""))
            .Where(u => u.Role == query.Role || query.Role == null)
            .Where(u => (bool)query.Enable! ? u.Deleted_at == null : u.Deleted_at != null)
            .Skip(query.Skip ?? 0)
            .Take(query.Take ?? 10)
            .ToListAsync();
    }
    public async Task<int> Count(UserQueryRequest query)
    {
        return await context.Users
            .Where(u => u.Name.Contains(query.Name ?? ""))
            .Where(u => u.Email.Contains(query.Email ?? ""))
            .Where(u => u.Register.Contains(query.Register ?? ""))
            .Where(u => (bool)query.Enable! ? u.Deleted_at == null : u.Deleted_at != null)
            .CountAsync();
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetById(string id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.ID == id);
    }

    public async Task<User?> GetByRegister(string register)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Register == register);

    }

    public async Task<User?> EnableOrDisable(User user)
    {
        var u = context.Users.Update(user);
        await context.SaveChangesAsync();
        return u.Entity;

    }

    public async Task<User?> Update(User user)
    {
        var u = context.Users.Update(user);
        await context.SaveChangesAsync();
        return u.Entity;
    }
}