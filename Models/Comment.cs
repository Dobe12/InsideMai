using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InsideMai.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public int LikesCount { get; set; }
        public int? AuthorId { get; set; }
        public int? PostId { get; set; }

        public bool IsDeleted { get; set; }

        public virtual User Author { get; set; }
        [JsonIgnore]
        public virtual Post Post { get; set; }

        public virtual ICollection<UserCommentLike> CommentLikes { get; set; }
    }
}
