using System;

namespace RewriteMe.WebApi.Models
{
    public class UploadFileItemModel
    {
        public string Name { get; set; }

        public string FileName { get; set; }

        public string Language { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
