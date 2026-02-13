using WapcGuest;

namespace Capabilities;
/// <summary>
/// Default implementation of the WapcClient interface
/// </summary>
public class WapcClient : IWapcClient
{
    /// <summary>
    /// Implementation of the HostCall method using wapc
    /// </summary>
    public byte[] HostCall(string binding, string nameSpace, string operation, byte[] payload)
    {
        // This would use the actual wapc library to make the host call
        return Wapc.HostCall(binding, nameSpace, operation, payload);
    }
}