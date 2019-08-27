using System;

namespace RewriteMe.WebApi.Dtos
{
    public class InformationMessageDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
