
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
