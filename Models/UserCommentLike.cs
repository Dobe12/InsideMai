using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsideMaiWebApi.Models
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
