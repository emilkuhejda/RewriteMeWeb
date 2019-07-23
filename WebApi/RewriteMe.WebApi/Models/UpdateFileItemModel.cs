using System;

namespace RewriteMe.WebApi.Models
{
    public class UpdateFileItemModel
    {
        public Guid FileItemId { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
