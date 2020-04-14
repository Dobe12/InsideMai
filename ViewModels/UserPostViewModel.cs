using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsideMaiWebApi.Models;

namespace InsideMaiWebApi.ViewModels
{
    public class UserPostViewModel : PostViewModel
    {
        public virtual ICollection<CommentViewModel> Comments { get; set; }
    }
}
