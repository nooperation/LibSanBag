using System.IO;
using LibSanBag.Providers;

namespace LibSanBag.ResourceUtils
{
    public static class Utils
    {
        /// <summary>
        /// Attempts to get the Sansar installation directory
        /// </summary>
        /// <param name="registryProvider">Registry provider</param>
        /// <returns>Sansar installation directory on success, otherwise null</returns>
        public static string GetSansarDirectory(IRegistryProvider registryProvider)
        {
            var installLocation = registryProvider.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Sansar", "InstallLocation", null) as string;
            if (installLocation == null)
            {
                var iconPath = registryProvider.GetValue(@"HKEY_CLASSES_ROOT\sansar\DefaultIcon", "", null) as string;
                if (iconPath != null)
                {
                    installLocation = Path.GetFullPath(iconPath + @"\..\..");
                }
            }

            return installLocation;
        }
    }
}
