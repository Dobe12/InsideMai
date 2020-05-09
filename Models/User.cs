using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace InsideMai.Models
{
    public class User : IdentityUser<int>
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Roles Role { get; set; }
        public string UserPic { get; set; }
        public bool IsDeleted { get; set; }
        public int? DepartmentId { get; set; }


        public virtual ICollection<User> Subscribers { get; set; }
        public virtual ICollection<Post> NewPosts { get; set; }


        public virtual Department Department { get; set; }
        [JsonIgnore]
        public virtual ICollection<Post> Posts { get; set; }
        [JsonIgnore]
        public virtual ICollection<Comment> Comments { get; set; }
        [JsonIgnore]
        public virtual ICollection<Favorite> Favorites { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserPostLike> PostLikes { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserCommentLike> CommentLikes { get; set; }
        public enum Roles
        {
            Admin,
            User,
            Lecturer,
            Employee,
            Student
        }
    }
}
