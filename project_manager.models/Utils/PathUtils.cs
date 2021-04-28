using System.Diagnostics;
using System.IO;

namespace project_manager.common.Utils
{
    public class PathUtils
    {
        public static string GetBasePath()
        {
            using var processModule = Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }
    }
}
