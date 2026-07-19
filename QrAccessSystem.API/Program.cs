using Microsoft.EntityFrameworkCore;
using QrAccessSystem.Persistence.Contexts;
using QrAccessSystem.Application.Interfaces;
using QrAccessSystem.Persistence.Repositories;
using FluentValidation;
using MediatR;
using QrAccessSystem.Application.Behaviors;
using QrAccessSystem.API.Middlewares;
using QrAccessSystem.API.Services;
using QrAccessSystem.API.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// JWT Kimlik Doğrulama Ayarları
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddEndpointsApiExplorer();
// Swagger'a "Authorize" (Kilit) butonunu ekleme
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "QrAccessSystem API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization başlığı. \r\n\r\n Aşağıdaki kutuya önce 'Bearer' yazıp boşluk bırakın, ardından token'ınızı yapıştırın. \r\n\r\nÖrnek: 'Bearer eyJhbGci...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

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
// SignalR ve Canlı Bildirim Servis Kaydı
builder.Services.AddSignalR();
builder.Services.AddScoped<IRealTimeService, RealTimeService>();
builder.Services.AddScoped<IAuthService, QrAccessSystem.Infrastructure.Services.AuthService>();

// CORS (Farklı portlardan gelen arayüz isteklerine izin verme)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .SetIsOriginAllowed(_ => true)
              .AllowCredentials(); // SignalR için zorunludur
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Global Hata Yakalayıcı Middleware'i devreye alıyoruz
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<AccessHub>("/access-hub");

app.Run();