using System;
using System.Collections.Generic;
using System.Text;

namespace project_manager.common.Utils
{
    public class ThreadUtils
    {
        public static void LockFunction(Action action, Type lockTypeOf)
        {
            lock (lockTypeOf)
            {
                action();
            }
        }
    }
}
