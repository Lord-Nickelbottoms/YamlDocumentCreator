using YamlDocumentCreator.Models;
using YamlDocumentCreator.Models.ViewModels;

namespace YamlDocumentCreator.Services
{
    public interface IAttachmentService
    {
        Task<Models.Attachment> UploadAttachment(AttachmentVM attachmentVM);
    }

    public class AttachmentService : IAttachmentService
    {
        private readonly AttachmentDbContext _dbContext;

        public AttachmentService(AttachmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Models.Attachment> UploadAttachment(AttachmentVM attachmentVM)
        {
            var newAttachment = new Attachment();

            if (attachmentVM.File.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await attachmentVM.File.CopyToAsync(memoryStream);

                    if (memoryStream.Length < 20971520)
                    {
                        newAttachment = new Attachment { Id = Guid.NewGuid().ToString(), FileName = attachmentVM.File.FileName, FileType = attachmentVM.File.ContentType, Content = memoryStream.ToArray(), UploadedDate = DateTime.UtcNow };
                        _dbContext.Attachment.Add(newAttachment);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            return newAttachment;
        }
    }
}