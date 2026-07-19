namespace QrAccessSystem.Application.Wrappers;

public class Response<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }

    // Başarılı cevaplar için
    public static Response<T> Success(T data, string message = "İşlem başarılı.")
    {
        return new Response<T> { Data = data, IsSuccess = true, Message = message };
    }

    // Hatalı cevaplar için
    public static Response<T> Fail(string errorMessage, List<string>? errors = null)
    {
        return new Response<T> { IsSuccess = false, Message = errorMessage, Errors = errors };
    }
}