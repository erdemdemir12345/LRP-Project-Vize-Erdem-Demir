using LabApi.Data;
using LabApi.Models;
using Microsoft.EntityFrameworkCore;
using LabApi.Endpoints;
using LabApi;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVİSLER ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=LabDatabase.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 2. ARA KATMANLAR ---
app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- 3. ENDPOINT BAĞLANTILARI ---
app.MapAuthEndpoints();
app.MapStatEndpoints();
app.MapLabEndpoints();
app.MapStudentEndpoints();

// --- 4. BİLGİSAYAR İŞLEMLERİ ---

// Listeleme (Tek bir tane yeterli)
app.MapGet("/api/bilgisayarlar", async (AppDbContext context) =>
    await context.Computers.ToListAsync());

// Ekleme
app.MapPost("/api/bilgisayar-ekle", async (AppDbContext context, Computer yeniPc) => {
    // 1. Gelen LabId'nin gerçekten var olup olmadığını kontrol et
    var lab = await context.Labs.FindAsync(yeniPc.LabId);
    if (lab == null) return Results.BadRequest("Geçersiz Laboratuvar ID!");

    // 2. Demirbaş Kodu Üretimi: Sadece bu lab'a ait PC'leri say
    var labPcCount = await context.Computers.CountAsync(c => c.LabId == yeniPc.LabId);

    // PC'nin kendi ID'sini değil, o lab içindeki sırasını kullanıyoruz
    yeniPc.AssetCode = $"LAB{yeniPc.LabId}-PC-{(labPcCount + 1):D2}";

    context.Computers.Add(yeniPc);
    await context.SaveChangesAsync();

    return Results.Ok(yeniPc);
});

// --- 5. VERİTABANI BAŞLATMA VE ADMİN SEED ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Veritabanı dosyasının ve tabloların oluştuğundan emin ol
    context.Database.EnsureCreated();

    if (!context.Users.Any(u => u.Role == "Admin"))
    {
        context.Users.Add(new User
        {
            Username = "erdem",
            Password = "7532159",
            Role = "Admin"
        });
        context.SaveChanges();
    }
}

app.Run();