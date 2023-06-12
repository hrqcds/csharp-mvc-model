namespace Utils;

public class HashPassword
{
    public static string Generate(string password)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        return passwordHash;
    }

    public static bool Verify(string password, string hash)
    {
        var result = BCrypt.Net.BCrypt.Verify(password, hash);

        return result;
    }
}

public class HashPasswordStruct
{
    public string Register { get; set; } = null!;
}