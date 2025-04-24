namespace Capabilities;
/// <summary>
/// Host makes possible to interact with the policy host from inside of a policy.
/// </summary>
public class Host
{
    public IWapcClient Client { get; set; }
}