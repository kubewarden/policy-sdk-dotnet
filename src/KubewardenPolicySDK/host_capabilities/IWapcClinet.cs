namespace Capabilities;

public interface IWapcClient
{
    byte[] HostCall(string binding, string nameSpace, string operation, byte[] payload);
}