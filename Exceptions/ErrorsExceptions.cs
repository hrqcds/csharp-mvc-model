namespace Exceptions;

public class ErrorExceptions : Exception
{
    public Dictionary<string, Object> Error { get; set; }

    public ErrorExceptions(string message, int statusCode, Dictionary<string, string[]> _Error) : base(message)
    {
        Error = new Dictionary<string, object>()
        {
            { "status", statusCode },
            { "title", message},
            { "errors", _Error }
        };

    }
}