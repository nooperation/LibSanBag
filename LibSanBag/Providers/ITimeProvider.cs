namespace LibSanBag.Providers
{
    /// <summary>
    /// Time provider interface. Used for providing a stable time source during testing.
    /// </summary>
    public interface ITimeProvider
    {
        /// <summary>
        /// Gets the current time in a Windows file time format
        /// </summary>
        /// <returns>Current time in a Windows file time format</returns>
        ulong GetCurrentTime();
    }
}
