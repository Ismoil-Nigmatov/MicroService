using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.ToDo.Dto;
using Service.ToDo.Entity.ENUMS;
using Service.ToDo.Repository;
using Service.ToDo.Service;
using Service.ToDo.Service.Request;

namespace Service.ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly EmailProducer _emailProducer;

        public TaskController(ITaskRepository taskRepository, IUserRepository userRepository, EmailProducer emailProducer)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _emailProducer = emailProducer;
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetTasks(int userId)
        {
            var tasks = await _taskRepository.GetAllTaskByUserId(userId);
            return Ok(tasks);
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddTask(TaskDTO taskDto)
        {
            var task = await _taskRepository.CreateTask(taskDto);
            EmailRequest email = new EmailRequest();
            email.ToEmail = await _userRepository.GetUserById(task.UserId);
            email.Subject = "Created new Task";
            email.Message =
                $"Created new Task => {task.Title}\n Description -> {task.Description} \n Status -> {Enum.GetName(typeof(EStatus), taskDto.Status)} \n Deadline -> {task.Deadline}";
            _emailProducer.SendEmailRequest(email);
            return Ok(task);
        }

        [HttpDelete("id")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            await _taskRepository.DeleteTask(id);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateTask(int id , TaskDTO taskDto)
        {
            await _taskRepository.UpdateTask(id, taskDto);
            EmailRequest email = new EmailRequest();
            email.ToEmail = await _userRepository.GetUserById(taskDto.UserId);
            email.Subject = "Updated an existing Task ";
            email.Message =
                $"Updated Task => Title - {taskDto.Title} \n Description - {taskDto.Description} \n Deadline - {taskDto.Deadline} \n Status - {Enum.GetName(typeof(EStatus), taskDto.Status)}";
            _emailProducer.SendEmailRequest(email);
            return Ok();
        }
    }
}
