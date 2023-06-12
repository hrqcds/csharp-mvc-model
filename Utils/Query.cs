using Models;

namespace Utils;

public class UserQueryRequest
{
    public int? Take { get; set; } = 25;
    public int? Skip { get; set; } = 0;
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Register { get; set; }
    public Roles? Role { get; set; }
    public bool? Enable { get; set; } = true;
}