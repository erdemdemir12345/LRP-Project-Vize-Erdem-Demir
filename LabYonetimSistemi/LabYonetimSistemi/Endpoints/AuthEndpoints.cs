using LabApi.Data;
using LabApi.Models;
using Microsoft.EntityFrameworkCore;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        // Giriş Endpoint'i
        routes.MapPost("/api/login", async (AppDbContext db, User loginData) => {
            // Kullanıcı adı ve şifre eşleşmesi kontrol edilir
            var user = await db.Users.FirstOrDefaultAsync(u =>
                u.Username.ToLower() == loginData.Username.ToLower() &&
                u.Password == loginData.Password);

            if (user == null) return Results.Unauthorized(); // 401: Yetkisiz

            // Başarılıysa kullanıcı adı ve rolü JSON olarak döner
            return Results.Ok(new { username = user.Username, role = user.Role });
        });
    }
}