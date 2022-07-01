using WapcGuest;
using KubewardenPolicySDK;

namespace Policy;

public class IngressPolicy
{
  public static void Main()
  {
    var wapc = new Wapc();
    wapc.RegisterFunction("protocol_version", Kubewarden.ProtocolVersionGuest);
    wapc.RegisterFunction("validate", Validator.Validate);
    wapc.RegisterFunction("validate_settings", PolicySettings.Validate);
  }
}

