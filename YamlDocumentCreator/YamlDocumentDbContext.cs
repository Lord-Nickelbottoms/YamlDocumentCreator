using Microsoft.EntityFrameworkCore;

// public class YamlDocumentDbContext(DbContextOptions<YamlDocumentDbContext> options) : DbContext(options)
// {
//     public DbSet<YamlDocumentCreator.Models.YamlDocument> YamlDocument { get; set; } = default!;
// }

public class AttachmentDbContext(DbContextOptions<AttachmentDbContext> options) : DbContext(options)
{
    public DbSet<YamlDocumentCreator.Models.Attachment> Attachment { get; set; } = default!;
}
