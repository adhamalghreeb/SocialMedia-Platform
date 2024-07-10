namespace Blog_Project.Models.DTO
{
    public class CreateCommentRequest
    {
        public string Content { get; set; }
        public Guid BlogPostId { get; set; }
    }
}
