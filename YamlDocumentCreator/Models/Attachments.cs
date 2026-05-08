using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YamlDocumentCreator.Models
{
    public class Attachments
    {
        [Key, MaxLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string FileType { get; set; }

        [Required, MaxLength(200)]
        public string FileName { get; set; }

        [Required]
        public byte[] Content { get; set; }

        [Required]
        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;
    }
}