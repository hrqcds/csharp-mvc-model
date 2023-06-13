using Exceptions;
using Models;
using Repositories;
using Utils;

namespace Services;

public class AuthService
{
    private readonly IUserRepository userRepository;
    private readonly IConfiguration configuration;
    private Dictionary<string, string[]> Error { get; set; } = new Dictionary<string, string[]>();

    public AuthService(IUserRepository _userRepository, IConfiguration _configuration)
    {
        userRepository = _userRepository;
        configuration = _configuration;
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var user = await userRepository.GetByRegister(request.Register);

        if (user == null)
        {
            Error.Add("User", new string[] { "User not found" });
            throw new ErrorExceptions("User not found", 404, Error);
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            Error.Add("Login", new string[] { "User or Password Incorrect" });
            throw new ErrorExceptions("Login Failure", 400, Error);
        }
        var gen = new GenerateToken(configuration);
        return new LoginResponse()
        {
            Token = gen.Execute(user),
            Register = user.Register,
            Role = gen.GetRole(user.Role)
        };
    }
}