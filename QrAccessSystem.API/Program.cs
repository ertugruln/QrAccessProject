using Microsoft.EntityFrameworkCore;
using QrAccessSystem.Persistence.Contexts;
using QrAccessSystem.Application.Interfaces;
using QrAccessSystem.Persistence.Repositories;
using FluentValidation;
using MediatR;
using QrAccessSystem.Application.Behaviors;
using QrAccessSystem.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL DbContext Entegrasyonu
builder.Services.AddDbContext<QrAccessDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR Kaydı (Marker Type yöntemi ile)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IGenericRepository<>).Assembly));

// AutoMapper Kaydı (Hatanın çözüldüğü yer)
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(IGenericRepository<>).Assembly));
// FluentValidation Kaydı
builder.Services.AddValidatorsFromAssembly(typeof(IGenericRepository<>).Assembly);

// MediatR Pipeline Behavior Kaydı
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
// Repository Pattern (IoC) Kaydı
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IQrCodeService, QrAccessSystem.Infrastructure.Services.QrCodeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Global Hata Yakalayıcı Middleware'i devreye alıyoruz
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();