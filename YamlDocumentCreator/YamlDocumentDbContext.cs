using Microsoft.EntityFrameworkCore;

public class YamlDocumentDbContext(DbContextOptions<YamlDocumentDbContext> options) : DbContext(options)
{
    public DbSet<YamlDocumentCreator.Models.YamlDocument> YamlDocument { get; set; } = default!;
}
