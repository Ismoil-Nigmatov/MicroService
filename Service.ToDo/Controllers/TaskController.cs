﻿using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.ToDo.Dto;
using Service.ToDo.Repository;

namespace Service.ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
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
            return Ok();
        }
    }
}