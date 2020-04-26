using System;
using RewriteMe.Domain.Dtos;

namespace RewriteMe.WebApi.Commands
{
    public class SendMailCommand : CommandBase<OkDto>
    {
        public Guid FileItemId { get; set; }

        public string Recipient { get; set; }
    }
}
