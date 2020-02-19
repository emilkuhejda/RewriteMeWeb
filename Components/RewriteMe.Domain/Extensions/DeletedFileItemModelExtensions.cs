using RewriteMe.Domain.Settings;

namespace RewriteMe.Domain.Extensions
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
