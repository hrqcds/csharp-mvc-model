using Models;
using Utils;

namespace Repositories;

public interface IUserRepository
{
    Task<User?> Create(User user);
    Task<User?> Update(User user);
    Task<User?> EnableOrDisable(User user);
    Task<List<User>> GetALL(UserQueryRequest query);
    Task<int> Count(UserQueryRequest query);
    Task<User?> GetById(string id);
    Task<User?> GetByRegister(string register);
    Task<User?> GetByEmail(string email);
}