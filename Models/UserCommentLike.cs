
namespace InsideMai.Models
{
    public class UserCommentLike
    {
        public int UserCommentLikeId { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }

        public Comment Comment { get; set; }
        public User User { get; set; }
    }
}
