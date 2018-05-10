namespace LibSanBag.Providers
{
    public interface IFileProvider
    {
        bool FileExists(string path);
    }
}
