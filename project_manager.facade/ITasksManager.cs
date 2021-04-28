using project_manager.common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_manager.contract
{
    public interface ITasksManager
    {
        System.Threading.Tasks.Task<IList<Task>> Get();
        System.Threading.Tasks.Task Create(Task model);
        System.Threading.Tasks.Task Edit(int id, Task model);
        System.Threading.Tasks.Task Delete(int id);
        System.Threading.Tasks.Task EndTask(int id);
    }
}
