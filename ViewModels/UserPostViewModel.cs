using System.Collections.Generic;

namespace InsideMai.ViewModels
{
    public class UserPostViewModel : PostViewModel
    {
        public virtual ICollection<CommentViewModel> Comments { get; set; }
    }
}
