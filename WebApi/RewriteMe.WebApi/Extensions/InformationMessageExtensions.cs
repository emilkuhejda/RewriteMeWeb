﻿using RewriteMe.Domain.Messages;
using RewriteMe.WebApi.Dtos;

namespace RewriteMe.WebApi.Extensions
{
    public static class InformationMessageExtensions
    {
        public static InformationMessageDto ToDto(this InformationMessage informationMessage)
        {
            return new InformationMessageDto
            {
                Id = informationMessage.Id,
                Title = informationMessage.Title,
                Message = informationMessage.Message,
                Description = informationMessage.Description,
                DateCreated = informationMessage.DateCreated
            };
        }
    }
}
