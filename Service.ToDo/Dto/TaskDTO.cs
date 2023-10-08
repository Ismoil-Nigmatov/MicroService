using Service.ToDo.Entity.ENUMS;
using Service.ToDo.Entity;

namespace Service.ToDo.Dto
{
    public class TaskDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public EStatus Status { get; set; }
        public int UserId { get; set; }
    }
}
