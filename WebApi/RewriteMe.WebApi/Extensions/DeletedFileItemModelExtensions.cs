using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Extensions
{
    public static class DeletedFileItemModelExtensions
    {
        public static DeletedFileItem ToDeletedFileItem(this DeletedFileItemModel model)
        {
            return new DeletedFileItem
            {
                Id = model.Id,
                DeletedDate = model.DeletedDate
            };
        }
    }
}
