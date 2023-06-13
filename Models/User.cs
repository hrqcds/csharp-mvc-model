using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string ID { get; set; } = null!;
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;
    [Required]
    public string Register { get; set; } = null!;
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;
    [Required]
    public Roles Role { get; set; } = Roles.TI;
    [Required]
    [JsonIgnore]
    public string Password { get; set; } = null!;
    [JsonIgnore]
    public DateTime? Created_at { get; set; }
    [JsonIgnore]
    public DateTime? Updated_at { get; set; }
    [JsonIgnore]
    public DateTime? Deleted_at { get; set; }


    public User()
    {
        this.Created_at = DateTime.UtcNow;
        this.Updated_at = DateTime.UtcNow;
    }
}

public enum Roles
{
    TI,
    ENG,
    MAT,
    ADM
}

public struct CreateUserRequest
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(150)]
    public string Name { get; set; }
    [Required]
    public string Register { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [MaxLength(100)]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Role is required")]
    [EnumDataType(typeof(Roles))]
    public Roles Role { get; set; }
}

public struct CreateUserResponse
{
    public string Message { get; set; }
    public string Password { get; set; }
}

public struct UpdateUserRequest
{
    [MaxLength(150)]
    public string? Name { get; set; }
    public string? Register { get; set; }
    [MaxLength(100)]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public string? Email { get; set; }
    [EnumDataType(typeof(Roles))]
    public Roles? Role { get; set; }
    [MinLength(6)]
    public string? Password { get; set; }
}

public class LoginRequest
{
    [Required]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public string Email { get; set; } = null!;
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;
}

public class LoginResponse
{
    public string Token { get; set; } = null!;
    public string Register { get; set; } = null!;
    public string Role { get; set; } = null!;
}