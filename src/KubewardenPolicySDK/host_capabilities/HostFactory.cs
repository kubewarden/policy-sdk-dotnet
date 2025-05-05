namespace Capabilities;
public static class HostFactory
{
    /// <summary>
    /// Creates a new Host with a WapcClient
    /// </summary>
    public static Host NewHost()
    {
        return new Host { Client = new WapcClient() };
    }
}