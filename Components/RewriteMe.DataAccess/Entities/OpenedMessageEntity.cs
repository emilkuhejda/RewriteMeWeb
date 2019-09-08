using System;

namespace RewriteMe.DataAccess.Entities
{
    public class OpenedMessageEntity
    {
        public Guid Id { get; set; }

        public Guid InformationMessageId { get; set; }

        public Guid UserId { get; set; }

        public virtual InformationMessageEntity InformationMessage { get; set; }
    }
}
