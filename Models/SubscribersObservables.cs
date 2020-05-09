using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InsideMai.Models
{
    public class SubscribersObservables
    {
        public int? SubscriberId { get; set; }
        public User Subscriber { get; set; }

        public int? ObservableId { get; set; }
        public User Observable { get; set; }

    }
}
