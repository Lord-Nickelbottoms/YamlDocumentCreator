using System.ComponentModel.DataAnnotations;

namespace YamlDocumentCreator.Models
{
    public class YamlDocument
    {
        [Required]
        public string Name { get; set; }

        [Key, MaxLength(36)]
        public string Id { get; set; }

        [Required]
        public string Group { get; set; }

        [Required]
        public bool UserCanDelete { get; set; }

        public AccessSftp AccessSftp { get; set; }
    }

    public class AccessSftp
    {
        [Required]
        public string Host { get; set; }

        [Required]
        public string Port { get; set; }
    }
}