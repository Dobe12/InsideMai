using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsideMaiWebApi.Models;

namespace InsideMaiWebApi.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public int LikesCount { get; set; }
        public int SavesCount { get; set; }
        public int CommentsCount { get; set; }
        public PostType Type { get; set; }
        public bool LikedByCurrentUser { get; set; }
        public bool AddedToFavByCurrentUser { get; set; }


        public virtual Department Department { get; set; }
        public virtual UserViewModel Author { get; set; }
    }
}
