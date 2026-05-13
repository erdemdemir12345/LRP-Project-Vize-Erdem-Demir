using LabApi.Data;
using LabApi.Models;
using Microsoft.EntityFrameworkCore;
namespace LabApi.Endpoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder routes)
    {
        // ÖĞRENCİ EKLEME VE ÖZEL ŞİFRE OLUŞTURMA
        routes.MapPost("/api/ogrenci-ekle", async (AppDbContext db, StudentRequest request) => {

            // 1. Öğrenci Tablosuna Kayıt
            var yeniOgrenci = new Student
            {
                FullName = request.FullName,
                Username = request.Username,
                Grade = request.Grade,
                ComputerId = request.ComputerId
            };
            db.Students.Add(yeniOgrenci);

            // 2. Kullanıcı Tablosuna Özel Şifre İle Kayıt
            var yeniKullanici = new User
            {
                Username = request.Username,
                Password = request.Password, // Admin'in belirlediği şifre
                Role = "Student"
            };
            db.Users.Add(yeniKullanici);

            await db.SaveChangesAsync();
            return Results.Ok(new { message = "Öğrenci ve hesabı oluşturuldu" });
        });

        // ÖĞRENCİ KENDİ PC'SİNİ GÖRÜR
        routes.MapGet("/api/ogrenci/bilgisayarim/{username}", async (string username, AppDbContext db) => {
            var ogrenci = await db.Students.FirstOrDefaultAsync(s => s.Username == username);
            if (ogrenci == null) return Results.NotFound();

            var pc = await db.Computers.FirstOrDefaultAsync(c => c.Id == ogrenci.ComputerId);
            return Results.Ok(pc);
        });

        routes.MapGet("/api/ogrenciler", async (AppDbContext db) => await db.Students.ToListAsync());
    }
}

// Veri transferi için yardımcı sınıf
public record StudentRequest(string FullName, string Username, string Password, int Grade, int ComputerId);