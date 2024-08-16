namespace Policy;

using KubewardenPolicySDK;

public class PolicySettings
{

    public static byte[] Validate(byte[] payload)
    {
        return Kubewarden.AcceptSettings();
    }
}

