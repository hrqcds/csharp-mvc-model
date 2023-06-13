using Exceptions;
using Generics;
using Models;
using Repositories;
using Utils;

namespace Services;

public class UserService
{
    private readonly IUserRepository userRepository;
    private Dictionary<string, string[]> Error { get; set; } = new Dictionary<string, string[]>();

    public UserService(IUserRepository _userRepository)
    {
        userRepository = _userRepository;
    }

    public async Task<DataResponse<User>> GetALL(UserQueryRequest query)
    {
        var users = await userRepository.GetALL(query);
        var count = await userRepository.Count(query);

        return new DataResponse<User>()
        {
            Data = users,
            Total = count
        };



    }

    public async Task<User> GetUserById(string id)
    {
        var user = await userRepository.GetById(id);

        if (user == null)
        {
            Error.Add("User", new string[] { "User not found" });
            throw new ErrorExceptions("User not found", 404, Error);
        }

        return user;
    }

    public async Task<CreateUserResponse?> Create(CreateUserRequest request)
    {
        var registerAlreadyExist = await userRepository.GetByRegister(request.Register);

        if (registerAlreadyExist != null)
        {
            Error.Add("Register", new string[] { "Register already exist" });
        }

        if (await userRepository.GetByEmail(request.Email) != null)
        {
            Error.Add("Email", new string[] { "Email already exist" });
        }

        if (Error.Count > 0)
            throw new ErrorExceptions("Error on create user", 400, Error);

        var password = Guid.NewGuid().ToString().Replace("-", "");
        if (password.Length > 8)
            password = password[..8];

        var password_hash = HashPassword.Generate(password);

        var user = new User()
        {
            Name = request.Name,
            Email = request.Email,
            Register = request.Register,
            Password = password_hash
        };

        await userRepository.Create(user);

        return new CreateUserResponse()
        {
            Message = "Users successfully created",
            Password = password
        };
    }

    public async Task<User?> Update(string id, UpdateUserRequest request)
    {
        var user = await userRepository.GetById(id);


        if (user == null)
        {
            Error.Add("User", new string[] { "User not found" });
            throw new ErrorExceptions("Error on update user", 404, Error);

        }
        else
        {

            if (user.Name != request.Name && request.Name != null)
            {
                user.Name = request.Name;
            }

            if (user.Register != request.Register && request.Register != null)
            {
                var registerAlreadyExist = await userRepository.GetByRegister(request.Register);

                if (registerAlreadyExist != null && registerAlreadyExist.ID != user.ID)
                {
                    Error.Add("Register", new string[] { "Register already exists in another user" });
                }
                else
                {
                    user.Register = request.Register;
                }

            }

            if (user.Email != request.Email && request.Email != null)
            {
                var emailAlreadyExist = await userRepository.GetByEmail(request.Email);

                if (emailAlreadyExist != null && emailAlreadyExist.ID != user.ID)
                {
                    Error.Add("Email", new string[] { "Email already exist in another user" });
                }
                else
                {
                    user.Email = request.Email;
                }

            }

            if (user.Role != request.Role && request.Role != null)
            {
                user.Role = (Roles)request.Role;
            }

            if (request.Password != null)
            {
                user.Password = HashPassword.Generate(request.Password);
            }
        }

        if (Error.Count > 0)
            throw new ErrorExceptions("Error on update user", 400, Error);

        user.Updated_at = DateTime.UtcNow;

        return await userRepository.Update(user);
    }
    public async Task<User?> Enable(string id)
    {
        var user = await userRepository.GetById(id);

        if (user == null)
        {
            Error.Add("User", new string[] { "User not found" });
            throw new ErrorExceptions("Error on delete user", 404, Error);
        }

        user.Updated_at = DateTime.UtcNow;
        user.Deleted_at = null;

        return await userRepository.EnableOrDisable(user);
    }

    public async Task<User?> Disable(string id)
    {
        var user = await userRepository.GetById(id);

        if (user == null)
        {
            Error.Add("User", new string[] { "User not found" });
            throw new ErrorExceptions("Error on delete user", 404, Error);
        }

        user.Updated_at = DateTime.UtcNow;
        user.Deleted_at = DateTime.UtcNow;

        return await userRepository.EnableOrDisable(user);
    }
}