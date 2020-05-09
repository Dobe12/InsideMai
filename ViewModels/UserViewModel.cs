using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsideMai.Models;

namespace InsideMai.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public virtual User.Roles Role { get; set; }
        public string UserPic { get; set; }
        public virtual string DepartmentName { get; set; }
        public bool? IsSubscribe { get; set; }
        public int NotificationsCount { get; set; }

    }
}
