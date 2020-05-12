﻿
namespace InsideMai.Models
{
    public class UserPostLike 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }

        public Post Post { get; set; }
        public User User { get; set; }
    }
}
