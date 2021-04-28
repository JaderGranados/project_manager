using project_manager.common.Models;
using System.Collections.Generic;

namespace project_manager.contract
{
    public interface IProjectsManager
    {
        System.Threading.Tasks.Task Create(Project model);
        System.Threading.Tasks.Task Edit(int id, Project model);
        System.Threading.Tasks.Task<IList<Project>> Get();
        System.Threading.Tasks.Task Delete(int id);
        System.Threading.Tasks.Task EndProject(int id);
    }
}
