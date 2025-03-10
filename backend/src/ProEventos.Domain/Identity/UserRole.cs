using Microsoft.AspNetCore.Identity;

namespace ProEventos.Domain.Identity;

public class UserRole : IdentityUserRole<int>
{
    public User User;
    public Role Role;
}
