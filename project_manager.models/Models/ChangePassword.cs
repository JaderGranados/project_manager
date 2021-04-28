using System;
using System.Collections.Generic;
using System.Text;

namespace project_manager.common.Models
{
    public class ChangePassword
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatNewPassowrd { get; set; }
    }
}
