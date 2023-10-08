using Service.ToDo.Dto;
using Task = Service.ToDo.Entity.Task;

namespace Service.ToDo.Repository
{
    public interface ITaskRepository
    {
        System.Threading.Tasks.Task<List<TaskDTO>> GetAllTaskByUserId(int  userId);

        System.Threading.Tasks.Task UpdateTask(int taskId, TaskDTO taskDto);

        System.Threading.Tasks.Task DeleteTask(int taskId);

        System.Threading.Tasks.Task<TaskDTO> CreateTask(TaskDTO taskDto);
    }
}
