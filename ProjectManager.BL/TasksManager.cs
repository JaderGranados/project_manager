using Microsoft.EntityFrameworkCore;
using project_manager.common.Enums;
using project_manager.common.Exceptions;
using project_manager.common.Models;
using project_manager.contract;
using project_manager.data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace project_manager.bl
{
    public class TasksManager : ITasksManager
    {
        public async System.Threading.Tasks.Task Create(Task model)
        {
            try
            {
                using (ApplicationDBContext _context = new ApplicationDBContext())
                {
                    var project = _context.Project.Find(model.ProjectId);
                    if (project == null)
                    {
                        throw new ServerException(400, "The specified project doesn't exist");
                    }
                    else if (!_context.User.Any(x => x.UserName == model.ModifierBy.Trim().ToLower()))
                    {
                        throw new ServerException(400, "The person who is modifying this task doesn't exist in database");
                    }
                    else if (project.StartDate > model.EjecutionDate)
                    {
                        throw new ServerException(400, "Ejecution date can't be smaller than starting date of project");
                    }
                    else if (project.EndDate < model.EjecutionDate)
                    {
                        throw new ServerException(400, "Ejecution date can't be bigger than ending date of project");
                    }

                    var newTask = new data.Models.Task
                    {
                        Description = model.Description,
                        EjecutionDate = model.EjecutionDate,
                        Name = model.Name,
                        ProjectId = model.ProjectId,
                        TaskState = TaskStateEnum.Pending,
                        CreatedAt = DateTime.Now,
                        CreatedBy = model.ModifierBy,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = model.ModifierBy
                    };

                    _context.Task.Add(newTask);
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                throw;
            }
        }

        public async System.Threading.Tasks.Task Delete(int id)
        {
            try
            {
                using(ApplicationDBContext _context = new ApplicationDBContext())
                {
                    var task = _context.Task.Find(id);
                    if(task == null)
                    {
                        throw new ServerException(400, "Task id is incorrect");
                    }

                    _context.Task.Remove(task);
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                throw;
            }
        }

        public async System.Threading.Tasks.Task Edit(int id, Task model)
        {
            try
            {
                using (ApplicationDBContext _context = new ApplicationDBContext())
                {
                    var task = _context.Task.Find(id);
                    if (task == null)
                    {
                        throw new ServerException(400, "Task id is not correct");
                    }
                    else if (!_context.User.Any(x => x.UserName == model.ModifierBy.Trim().ToLower()))
                    {
                        throw new ServerException(400, "The person who is modifying this task doesn't exist in database");
                    }

                    var project = _context.Project.Find(task.ProjectId);
                    if (project.StartDate > model.EjecutionDate)
                    {
                        throw new ServerException(400, "Ejecution date can't be smaller than starting date of project");
                    }
                    else if (project.EndDate < model.EjecutionDate)
                    {
                        throw new ServerException(400, "Ejecution date can't be bigger than ending date of project");
                    }

                    task.Description = model.Description ?? task.Description;
                    task.EjecutionDate = model.EjecutionDate;
                    task.Name = model.Name ?? task.Name;
                    task.UpdatedAt = DateTime.Now;
                    task.UpdatedBy = model.ModifierBy;

                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                throw;
            }
        }

        public async System.Threading.Tasks.Task EndTask(int id)
        {
            try
            {
                using (ApplicationDBContext _context = new ApplicationDBContext())
                {
                    var task = _context.Task.Find(id);
                    
                    if (task == null)
                    {
                        throw new ServerException(400, "Task id is not correct");
                    }
                    else if (task.TaskState == TaskStateEnum.Done)
                    {
                        throw new ServerException(400, "Task is already ended");
                    }

                    task.TaskState = TaskStateEnum.Done;
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                throw;
            }
        }

        public async System.Threading.Tasks.Task<IList<Task>> Get()
        {
            try
            {
                using (ApplicationDBContext _context = new ApplicationDBContext())
                {
                    return await _context.Task.Select(x => new Task
                    {
                        TaskId = x.TaskId,
                        Description = x.Description,
                        EjecutionDate = x.EjecutionDate,
                        Name = x.Name,
                        ProjectId = x.ProjectId,
                        TaskState = x.TaskState
                    }).ToListAsync();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
