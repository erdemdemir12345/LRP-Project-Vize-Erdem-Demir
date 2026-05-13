namespace LabApi.Models;

// 1. KULLANICI & AUTH TABLOSU
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } // Giriş adı (Örn: öğrenci numarası veya 'admin')
    public string Password { get; set; } // Şifre
    public string Role { get; set; }     // "Admin" veya "Student"
}

// 2. LABORATUVAR TABLOSU
public class Lab
{
    public int Id { get; set; }
    public string Name { get; set; } // Örn: "Lab-1"
    public List<Computer> Computers { get; set; } = new();
}

// 3. BİLGİSAYAR TABLOSU
public class Computer
{
    public int Id { get; set; }
    public string? AssetCode { get; set; }
    public string Brand { get; set; }
    public string Processor { get; set; }
    public int Ram { get; set; }
    public bool HasHdmi { get; set; }
    public bool HasInternet { get; set; }
    public bool HasVeyon { get; set; }

    public int LabId { get; set; } // Foreign Key
}

// 4. ÖĞRENCİ TABLOSU
public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int Grade { get; set; } // 1 veya 2. sınıf
    public int ComputerId { get; set; } // Sorumlu olduğu PC
    public string Username { get; set; } // User tablosuyla eşleşecek kullanıcı adı
}

// 5. YAZILIM ENVANTERİ
public class Software
{
    public int Id { get; set; }
    public string Name { get; set; } // Örn: "Visual Studio 2022"
    public bool IsRequired { get; set; } // Zorunlu mu?
}

// 6. ARIZA / SORUN KAYDI
public class Issue
{
    public int Id { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public bool IsResolved { get; set; } // Çözüldü mü?
    public int ComputerId { get; set; }
}