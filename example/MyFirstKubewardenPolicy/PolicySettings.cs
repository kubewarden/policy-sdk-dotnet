namespace Policy;

using KubewardenPolicySDK;
using System.Text.Json;
using System.Text.Json.Serialization;

class PolicySettings
{
  [JsonPropertyName("wipe_default_backend")]
  public bool? WipeDefaultBackend { get; set; }

  public static byte[] Validate(byte[] payload)
  {
    try
    {
      PolicySettings? policySettings = JsonSerializer.Deserialize<PolicySettings>(payload);
      if (policySettings == null)
      {
        return Kubewarden.RejectSettings("Null settings");
      }

      return Kubewarden.AcceptSettings();
    }
    catch (Exception e)
    {
      return Kubewarden.RejectSettings(
        $"Invalid JSON input for settings: {e}"
      );
    }
  }
}