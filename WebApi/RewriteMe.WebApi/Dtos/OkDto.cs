using System;

namespace RewriteMe.WebApi.Dtos
{
    public class OkDto
    {
        public OkDto()
            : this(DateTime.UtcNow)
        {
        }

        public OkDto(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public DateTime DateTime { get; }
    }
}
