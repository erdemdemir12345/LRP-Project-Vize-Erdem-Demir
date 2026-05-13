using LabApi.Data;
using Microsoft.EntityFrameworkCore; // CountAsync için bu satır şart

public static class StatEndpoints
{
    public static void MapStatEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/stats", async (AppDbContext db) => {
            var stats = new
            {
                labCount = await db.Labs.CountAsync(),
                pcCount = await db.Computers.CountAsync(),
                studentCount = await db.Students.CountAsync(),
                // ÇÖZÜM: CountAsync kullanarak asenkron hale getirdik
                issueCount = await db.Issues.CountAsync(i => !i.IsResolved)
            };
            return Results.Ok(stats);
        });
    }
}