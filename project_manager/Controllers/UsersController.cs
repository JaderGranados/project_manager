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
    [Authorize(Roles = "Operador,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersManager _usersManager;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public UsersController(IUsersManager usersManager, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _usersManager = usersManager;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var response = new Response<IList<User>>
            {
                Success = true
            };
            try
            {
                response.Data = _usersManager.Get();
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                response.Success = false;
                response.Message = e.Message;
            }
            return new JsonResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("activate-desactivate/{id}")]
        public async System.Threading.Tasks.Task<IActionResult> ActivateDesactivate(int? id)
        {
            var response = new Response<string>
            {
                Success = true
            };
            try
            {
                if (id == null)
                {
                    throw new ServerException(400, "User id is required");
                }
                response.Data = (await _usersManager.ActivateDesactivate(id.Value)) ? "User has been activate" : "User has been desactivate";
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
        [CustomAuthorization(Roles = "Admin")]
        public async System.Threading.Tasks.Task<IActionResult> Post([FromBody, Bind(include: "Name,LastName,UserName,Password,Email")] User model)
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
                await _usersManager.Create(model);
                response.Data = "User created successfully";
                HttpContext.Response.StatusCode = 201;
            }
            catch(ServerException e)
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

        [HttpPost("change-password")]
        public async System.Threading.Tasks.Task<IActionResult> ChangePassword([FromBody, Bind(include: "Username,OldPassword,NewPassword,RepeatNewPassowrd")] ChangePassword model)
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
                await _usersManager.ChangePassword(model);
                response.Data = "Password has been changed";
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

        [AllowAnonymous]
        [HttpPost, Route("/api/authenticate")]
        public IActionResult Authenticate([FromBody] Login userCredentials)
        {
            var response = new Response<string>
            {
                Success = true
            };
            try
            {
                string token = _jwtAuthenticationManager.Authenticate(userCredentials.Username, userCredentials.Password);
                response.Data = token;
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
