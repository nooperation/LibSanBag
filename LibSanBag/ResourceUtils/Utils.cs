using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.ResourceUtils
{
    static class Utils
    {
        /// <summary>
        /// Attempts to get the Sansar installation directory
        /// </summary>
        /// <returns>Sansar installation directory on success, otherwise null</returns>
        public static string GetSansarDirectory()
        {
            var installLocation = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Sansar", "InstallLocation", null) as string;
            if (installLocation == null)
            {
                var iconPath = Registry.GetValue(@"HKEY_CLASSES_ROOT\sansar\DefaultIcon", "", null) as string;
                installLocation = Path.GetFullPath(iconPath + @"\..\..");
            }

            return installLocation;
        }
    }
}
