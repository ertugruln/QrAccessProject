using System.Net;
using System.Text.Json;
using QrAccessSystem.Application.Exceptions;
using QrAccessSystem.Application.Wrappers;

namespace QrAccessSystem.API.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            // İsteği normal şekilde çalıştır
            await _next(context);
        }
        catch (Exception error)
        {
            // Bir hata çıkarsa (catch) buraya düşer
            var response = context.Response;
            response.ContentType = "application/json";

            var responseModel = Response<string>.Fail(error.Message);

            switch (error)
            {
                case ValidationException e:
                    // Bizim fırlattığımız doğrulama hatası (400 Bad Request)
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.Message = "Doğrulama hataları oluştu.";
                    responseModel.Errors = e.Errors;
                    break;
                default:
                    // Beklenmedik sunucu hatası (500 Internal Server Error)
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }
    }
}