using Service.ToDo.Entity.ENUMS;

namespace Service.ToDo.Entity
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public EStatus Status { get; set; }
        public User User { get; set; }
    }
}
