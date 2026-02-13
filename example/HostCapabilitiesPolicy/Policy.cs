using KubewardenPolicySDK;
using WapcGuest;

namespace Policy
{
    public class HostCapPolicy
    {
        public static void Main()
        {
            var wapc = new Wapc();
            wapc.RegisterFunction("protocol_version", Kubewarden.ProtocolVersionGuest);
            wapc.RegisterFunction("validate", PolicyRules.Validate);
            wapc.RegisterFunction("validate_settings", PolicySettings.Validate);
        }
    }
}
