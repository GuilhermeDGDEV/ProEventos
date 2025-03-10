using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Contextos;
using ProEventos.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace ProEventos.Persistence;

public class UserPersist(ProEventosContext context) : GeralPersist(context), IUserPersist
{
    private readonly ProEventosContext _context = context;

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .SingleOrDefaultAsync(user => user.Id == id);
    }

    public async Task<User> GetUserByUserNameAsync(string userName)
    {
        return await _context.Users
            .SingleOrDefaultAsync(user => user.UserName.ToLower() == (userName ?? "").ToLower());
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
}