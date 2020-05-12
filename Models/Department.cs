using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace InsideMai.Models
{
    public class Department
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }

        [JsonIgnore]
        public virtual Department Parent { get; set; }
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
        [JsonIgnore]
        public virtual ICollection<Post> Posts { get; set; }

    }

    public enum DepartmentLevel
    {
        MaiLvl = 1,
        UniversityLvl = 2,
        DepartmentLvl = 3,
        GroupLvl = 4
    }
}
