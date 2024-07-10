namespace Blog_Project.CORE.Models.DTO
{
    public class CreateCommentRequest
    {
        public string Content { get; set; }
        public Guid BlogPostId { get; set; }
    }
}
