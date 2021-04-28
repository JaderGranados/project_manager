using project_manager.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using project_manager.data.Context;
using Microsoft.EntityFrameworkCore;
using project_manager.emailing;
using project_manager.common.Exceptions;
using project_manager.common.Models;
using project_manager.common.Utils;

namespace project_manager.bl
{
    public class UsersManager : IUsersManager
    {
        public IList<User> Get()
        {
            try
            {
                using (ApplicationDBContext _context = new ApplicationDBContext())
                {
                    return _context.User.Include(x => x.Roles).ThenInclude(x => x.Role).Select(x => new User
                    {
                        UserId = x.UserId,
                        Email = x.Email,
                        IsActive = x.IsActive,
                        Name = x.Name,
                        LastName = x.LastName,
                        UserName = x.UserName,
                        Roles = x.Roles.Select(m => new Role
                        {
                            RoleId = m.RoleId,
                            Name = m.Role.Name
                        }).ToList()
                    }).ToList();
                }
            }
            catch {
                throw;
            }
        }

        public async System.Threading.Tasks.Task<bool> ActivateDesactivate(int id)
        {
            try
            {
                using (ApplicationDBContext _context = new ApplicationDBContext())
                {
                    var user = _context.User.Include(x => x.Roles).ThenInclude(x => x.Role).Where(x => x.UserId == id).FirstOrDefault();
                    if (user == null)
                    {
                        throw new ServerException(400, "User id is incorrect");
                    }
                    else if (user.Roles.Any(x => x.Role.Name == Roles.ADMIN))
                    {
                        throw new ServerException(400, "Can't desactivate admin users");
                    }
                    user.IsActive = !user.IsActive;

                    await _context.SaveChangesAsync();
                    return user.IsActive;
                }
            }
            catch {
                throw;
            }
        }

        public async System.Threading.Tasks.Task Create(User model)
        {
            try
            {
                using (ApplicationDBContext _context = new ApplicationDBContext())
                {
                    if (_context.User.Any(x => x.UserName == model.UserName.Trim().ToLower()))
                    {
                        throw new ServerException(400, "Username is been used.");
                    }

                    var newUser = new data.Models.User()
                    {
                        Email = model.Email.Trim(),
                        LastName = model.LastName.Trim(),
                        Name = model.Name.Trim(),
                        Password = model.Password.Trim(),
                        UserName = model.UserName.Trim().ToLower()
                    };

                    var rolOperador = _context.Role.Where(x => x.Name == Roles.OPERATOR).FirstOrDefault();
                    if (rolOperador == null)
                    {
                        throw new ServerException(500, "Server error");
                    }
                    newUser.Roles.Add(new data.Models.UserRole
                    {
                        RoleId = rolOperador.RoleId
                    });


                    _context.User.Add(newUser);
                    await _context.SaveChangesAsync();

                    // This call is unawaited for not spending more time. Send email in parallel
                    ThreadUtils.LockFunction(() => EmailHelper.SendEmail(new EmailModel("Se ha creado su nuevo usuario", $"Usuario: {model.UserName}<br/>Contraseña: {model.Password}<br/>", model.Email)), typeof(EmailHelper));
                }
            }
            catch {
                throw;
            }
        }

        public async System.Threading.Tasks.Task ChangePassword(ChangePassword model)
        {
            try
            {
                using (ApplicationDBContext _context = new ApplicationDBContext())
                {
                    if (model.NewPassword.Trim() != model.RepeatNewPassowrd.Trim())
                    {
                        throw new ServerException(400, "New and old password must be equal");
                    }

                    var user = _context.User.Where(x => x.UserName == model.Username.Trim().ToLower() && x.Password == model.OldPassword.Trim()).FirstOrDefault();

                    if (user == null)
                    {
                        throw new ServerException(400, "Username and password don't match");
                    }

                    user.Password = model.NewPassword.Trim();

                    await _context.SaveChangesAsync();
                }
            }
            catch {
                throw;
            }
        }

    }
}
