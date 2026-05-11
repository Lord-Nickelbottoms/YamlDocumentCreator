using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YamlDocumentCreator.Models.ViewModels
{
    public class AttachmentVM
    {
        public string Id { get; set; }
        public IFormFile File { get; set; }
    }
}