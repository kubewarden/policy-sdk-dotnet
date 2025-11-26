using Capabilities;
namespace Net;

public interface INetworkOperations
{
    List<string> LookupHost(Host host, string hostname);
}
