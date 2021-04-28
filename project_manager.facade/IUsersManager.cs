using project_manager.common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_manager.contract
{
    public interface IUsersManager
    {
        IList<User> Get();
        System.Threading.Tasks.Task Create(User model);
        System.Threading.Tasks.Task ChangePassword(ChangePassword model);
        System.Threading.Tasks.Task<bool> ActivateDesactivate(int id);
    }
}
