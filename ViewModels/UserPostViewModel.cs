using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsideMai.Models;

namespace InsideMai.ViewModels
{
    public class UserPostViewModel : PostViewModel
    {
        public virtual ICollection<CommentViewModel> Comments { get; set; }
    }
}
