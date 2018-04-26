using Microsoft.Win32;

namespace LibSanBag.Providers
{
    public class RegistryProvider : IRegistryProvider
    {
        public object GetValue(string keyName, string valueName, object defaultValue)
        {
            return Registry.GetValue(keyName, valueName, defaultValue);
        }
    }
}
