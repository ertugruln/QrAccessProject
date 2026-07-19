namespace QrAccessSystem.Application.Exceptions;

public class ValidationException : Exception
{
    public List<string> Errors { get; }

    public ValidationException() : base("Bir veya birden fazla doğrulama hatası oluştu.")
    {
        Errors = new List<string>();
    }

    public ValidationException(IEnumerable<string> failures) : this()
    {
        Errors = failures.ToList();
    }
}