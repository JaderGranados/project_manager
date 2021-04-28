using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_manager.common.Exceptions;
using project_manager.common.Models;
using project_manager.contract;
using project_manager.Tools;
using System;
using System.Collections.Generic;

namespace project_manager.Controllers
{
    [Authorize(Roles = "Admin,Operador")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsManager _projectsManager;

        public ProjectsController(IProjectsManager projectsManager)
        {
            _projectsManager = projectsManager;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            var response = new Response<string>()
            {
                Success = true
            };
            try
            {
                if (id == null)
                {
                    throw new ServerException(400, "Project id is required");
                }
                _projectsManager.Delete(id.Value);
                response.Data = "The project and his tasks have been deleted successfully";
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
                response.Success = false;
                response.Message = e.Message;
            }
            return new JsonResult(response);
        }

        [HttpGet("end-project/{id}")]
        public async System.Threading.Tasks.Task<IActionResult> EndProject(int? id)
        {
            var response = new Response<string>()
            {
                Success = true
            };
            try
            {
                if (id == null)
                {
                    throw new ServerException(400, "Project id is required");
                }
                await _projectsManager.EndProject(id.Value);
                response.Data = "The project has been ended successfully";
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
                response.Success = false;
                response.Message = e.Message;
            }
            return new JsonResult(response);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> Get()
        {
            var response = new Response<IList<Project>>()
            {
                Success = true
            };
            try
            {
                response.Data = await _projectsManager.Get();
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
                response.Success = false;
                response.Message = e.Message;
            }
            return new JsonResult(response);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Post([FromBody, Bind(include: "Name,Description,StartDate,EndDate,ModiffierName")] Project model)
        {
            var response = new Response<string>
            {
                Success = true
            };
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ServerException(400, "Todos los campos son requeridos.");
                }
                await _projectsManager.Create(model);
                response.Data = "Project created successfully.";
                HttpContext.Response.StatusCode = 201;
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
        public async System.Threading.Tasks.Task<IActionResult> Put(int? id, [FromBody, Bind(include: "Name,Description,EndDate,ModiffierName")] Project model)
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
                model.StartDate = DateTime.Now;
                if (!ModelState.IsValid)
                {
                    throw new ServerException(400, "Todos los campos son requeridos.");
                }
                await _projectsManager.Edit(id.Value, model);
                response.Data = "Project updated successfully.";
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
    }
}
