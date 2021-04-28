using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project_manager.common.Exceptions;
using project_manager.common.Models;
using project_manager.contract;
using System;
using System.Collections.Generic;

namespace project_manager.Controllers
{
    [Authorize(Roles = "Admin,Operador")]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksManager _tasksManager;

        public TasksController(ITasksManager tasksManager)
        {
            _tasksManager = tasksManager;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> Get()
        {
            var response = new Response<IList<Task>>
            {
                Success = true
            };
            try
            {
                response.Data = await _tasksManager.Get();
            }
            catch (ServerException e)
            {
                HttpContext.Response.StatusCode = e.StatusCode;
                response.Success = false;
                response.Message = e.Message;
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                response.Success = false;
                response.Message = e.Message;
            }
            return new JsonResult(response);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Post([FromBody, Bind(include: "Name,Description,EjecutionDate,ProjectId")] Task model)
        {
            var response = new Response<string>
            {
                Success = true
            };
            try
            {
                await _tasksManager.Create(model);
                HttpContext.Response.StatusCode = 201;
                response.Data = "Task created successfully";
            }
            catch (ServerException e)
            {
                HttpContext.Response.StatusCode = e.StatusCode;
                response.Success = false;
                response.Message = e.Message;
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                response.Success = false;
                response.Message = e.Message;
            }
            return new JsonResult(response);
        }

        [HttpPut("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> Put(int? id, [FromBody, Bind(include: "Name,Description,EjecutionDate,ProjectId")] Task model)
        {
            var response = new Response<string>
            {
                Success = true
            };
            try
            {
                if (id == null)
                {
                    throw new ServerException(400, "Project id is required");
                }
                await _tasksManager.Edit(id.Value, model);
                response.Data = "Task updated successfully";
            }
            catch (ServerException e)
            {
                HttpContext.Response.StatusCode = e.StatusCode;
                response.Success = false;
                response.Message = e.Message;
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                response.Success = false;
                response.Message = e.Message;
            }
            return new JsonResult(response);
        }

        [HttpDelete("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> Delete(int? id)
        {
            var response = new Response<string>
            {
                Success = true
            };
            try
            {
                if(id == null)
                {
                    throw new ServerException(400, "Task id required");
                }
                await _tasksManager.Delete(id.Value);
                response.Data = "Task deleted successfully";
            }
            catch (ServerException e)
            {
                HttpContext.Response.StatusCode = e.StatusCode;
                response.Success = false;
                response.Message = e.Message;
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                response.Success = false;
                response.Message = e.Message;
            }
            return new JsonResult(response);
        }

        [HttpGet("end-task/{id}")]
        public async System.Threading.Tasks.Task<IActionResult> EndTask(int? id)
        {
            var response = new Response<string>
            {
                Success = true
            };
            try
            {
                if (id == null)
                {
                    throw new ServerException(400, "Task id is required");
                }

                await _tasksManager.EndTask(id.Value);
                response.Data = "Task has been ended successfully";
            }
            catch (ServerException e)
            {
                HttpContext.Response.StatusCode = e.StatusCode;
                response.Message = e.Message;
                response.Success = false;
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                response.Message = e.Message;
                response.Success = false;
            }
            return new JsonResult(response);
        }
    }
}
