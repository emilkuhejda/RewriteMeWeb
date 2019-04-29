using System;

namespace RewriteMe.WebApi.Dtos
{
    public class AudioSourceDto
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public string ContentType { get; set; }

        public int Version { get; set; }
    }
}
