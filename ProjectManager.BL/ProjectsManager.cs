using Microsoft.EntityFrameworkCore;
using project_manager.common.Enums;
using project_manager.common.Exceptions;
using project_manager.common.Models;
using project_manager.common.Utils;
using project_manager.contract;
using project_manager.data.Context;
using project_manager.emailing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project_manager.bl
{
    public class ProjectsManager : IProjectsManager
    {
        public async System.Threading.Tasks.Task Create(Project model)
        {
            try
            {
                using (ApplicationDBContext _context = new ApplicationDBContext())
                {
                    if (!_context.User.Any(x => x.UserName == model.ModiffierName.Trim().ToLower()))
                    {
                        throw new ServerException(400, "The person who is triying to create this project doesn't exist");
                    }
                    else if (model.StartDate < DateTime.Now)
                    {
                        throw new ServerException(400, "Starting date must be bigger or equal than current date");
                    }
                    else if (model.StartDate >= model.EndDate)
                    {
                        throw new ServerException(400, "Starting date can't be bigger or equal than ending date");
                    }
                    var newProject = new data.Models.Project
                    {
                        Name = model.Name.Trim(),
                        Description = model.Description.Trim(),
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = model.ModiffierName.Trim(),
                        UpdatedBy = model.ModiffierName.Trim(),
                        ProjectState = ProjectStateEnum.InProcess
                    };

                    _context.Project.Add(newProject);

                    await _context.SaveChangesAsync();
                }
            }
            catch {
                throw;
            }
        }

        public async System.Threading.Tasks.Task Delete(int id)
        {
            try
            {
                using (ApplicationDBContext _context = new ApplicationDBContext())
                {
                    var project = _context.Project.Include(x => x.Tasks).Where(x => x.ProjectId == id).FirstOrDefault();
                    if (project == null)
                    {
                        throw new ServerException(400, $"The project specified by id {id}, doesn't exist in database");
                    }

                    project.Tasks.Clear();
                    _context.Project.Remove(project);

                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                throw;
            }
        }

        public async System.Threading.Tasks.Task Edit(int id, Project model)
        {
            try
            {
                using (ApplicationDBContext _context = new ApplicationDBContext())
                {
                    var oldProject = _context.Project.Include(x => x.Tasks).Where(x => x.ProjectId == id).FirstOrDefault();
                    if (!_context.User.Any(x => x.UserName == model.ModiffierName.Trim().ToLower()))
                    {
                        throw new ServerException(400, "The person who is triying to modify this project doesn't exist");
                    }
                    else if (oldProject == null)
                    {
                        throw new ServerException(400, "Project id is incorrect");
                    }
                    else if (oldProject.StartDate >= model.EndDate)
                    {
                        throw new ServerException(400, "Starting date can't be bigger or equal than ending date");
                    }
                    else if (oldProject.Tasks.Any(x => x.EjecutionDate > model.EndDate))
                    {
                        throw new ServerException(400, "This project has tasks with ejection date bigger than the specified ending date");
                    }
                    oldProject.Name = model.Name?.Trim() ?? oldProject.Name;
                    oldProject.Description = model.Description?.Trim() ?? oldProject.Description;
                    oldProject.EndDate = model.EndDate;
                    oldProject.UpdatedAt = DateTime.Now;
                    oldProject.UpdatedBy = model.ModiffierName;

                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                throw;
            }
        }

        public async System.Threading.Tasks.Task EndProject(int id)
        {
            try
            {
                using(ApplicationDBContext _context = new ApplicationDBContext())
                {
                    var project = _context.Project.Include(x => x.Tasks).Where(x => x.ProjectId == id).FirstOrDefault();

                    if (project == null)
                    {
                        throw new ServerException(400, "Project id is incorrect");
                    }
                    else if (project.ProjectState == ProjectStateEnum.Ended)
                    {
                        throw new ServerException(400, "Project is already ended");
                    }
                    else if (project.Tasks.Any(x => x.TaskState != TaskStateEnum.Done))
                    {
                        throw new ServerException(400, "Project has pending tasks");
                    }
                    project.ProjectState = ProjectStateEnum.Ended;

                    await _context.SaveChangesAsync();

                    await _context.User.Where(x => x.Roles.Any(m => m.Role.Name == Roles.ADMIN)).ForEachAsync(user =>
                    {
                        var emailModel = new EmailModel
                        {
                            Subject = $"The project {project.Name} has been ended",
                            Body = $"The project {project.Name} is ended",
                            Email = user.Email,
                            IsHtml = true
                        };
                        ThreadUtils.LockFunction(() => EmailHelper.SendEmail(emailModel), typeof(EmailModel));
                    });
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<IList<Project>> Get()
        {
            try
            {
                using(ApplicationDBContext _context = new ApplicationDBContext())
                {
                    return await _context.Project.Include(x => x.Tasks).Select(x => new Project
                    {
                        Description = x.Description,
                        EndDate = x.EndDate,
                        Name = x.Name,
                        ProjectId = x.ProjectId,
                        ProjectState = x.ProjectState,
                        StartDate = x.StartDate,
                        Tasks = x.Tasks.Select(m => new common.Models.Task
                        {
                            TaskId = m.TaskId,
                            Description = m.Description,
                            EjecutionDate = m.EjecutionDate,
                            Name = m.Name,
                            TaskState = m.TaskState,
                            ModifierBy = m.UpdatedBy
                        }).ToList()
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
