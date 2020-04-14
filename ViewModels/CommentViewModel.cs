using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsideMaiWebApi.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public int LikesCount { get; set; }
        public bool? LikedByCurrentUser { get; set; }
        public UserViewModel Author { get; set; }
    }
}
