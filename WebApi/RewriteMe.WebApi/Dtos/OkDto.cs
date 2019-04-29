namespace RewriteMe.WebApi.Dtos
{
    public class OkDto
    {
        public OkDto()
        {
            IsSuccess = true;
        }

        public bool IsSuccess { get; set; }
    }
}
