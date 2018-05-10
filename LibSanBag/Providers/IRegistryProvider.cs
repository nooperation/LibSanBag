namespace LibSanBag.Providers
{
    public interface IRegistryProvider
    {
        object GetValue(string keyName, string valueName, object defaultValue);
    }
}
