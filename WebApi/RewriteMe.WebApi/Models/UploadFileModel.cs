using System;

namespace RewriteMe.WebApi.Models
{
    public class UploadFileModel
    {
        public Guid FileItemId { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }

        public int FileItemVersion { get; set; }

        public int SourceVersion { get; set; }
    }
}
