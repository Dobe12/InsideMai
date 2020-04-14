using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsideMaiWebApi.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public int LikesCount { get; set; }
        public int SavesCount { get; set; }
        public int CommentsCount { get; set; }
        public int? AuthorId { get; set; }
        public int? DepartmentId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAnonymous { get; set; }
        public PostType Type { get; set; }


        public virtual Department Department { get; set; }
        public virtual User Author { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }

        public virtual ICollection<UserPostLike> PostLikes { get; set; }
    }

    public enum PostType
    {
        All = 1,
        Question = 2,
        Article = 3,
        Advert = 4,
        Event = 5
    }

}
