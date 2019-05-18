using System;

namespace RewriteMe.Domain.Settings
{
    public class DeletedFileItem
    {
        public Guid Id { get; set; }

        public DateTime DeletedDate { get; set; }
    }
}
