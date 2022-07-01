namespace Policy;

using KubewardenPolicySDK;
using System.Text.Json;
using System.Text.Json.Serialization;
using k8s.Models;


class Validator
{
  public static byte[] Validate(byte[] payload)
  {
    try
    {
      ValidationRequest? validationRequest = JsonSerializer.Deserialize<ValidationRequest>(payload);

      if (validationRequest is ValidationRequest req)
      {
        PolicySettings? policySettings = req.Settings.Deserialize<PolicySettings>();

        bool wipeDefaultBackend = policySettings?.WipeDefaultBackend ?? false;

        return ProcessValidationRequest(ref req, wipeDefaultBackend);
      }
      else
      {
        return Kubewarden.RejectRequest("Invalid payload", 400, null, null);
      }

    }
    catch (Exception e)
    {
      return Kubewarden.RejectRequest($"Internal errror: {e}", 500, null, null);
    }
  }

  private static byte[] ProcessValidationRequest(ref ValidationRequest req, bool wipeDefaultBackend)
  {
    V1Ingress? maybeIngress = req.Request.Object?.Deserialize<V1Ingress>();
    if (maybeIngress is V1Ingress ingress)
    {
      if (ingress.Spec.DefaultBackend != null)
      {
        if (wipeDefaultBackend)
        {
          ingress.Spec.DefaultBackend = null;

          var jsonSerializerOptions = new JsonSerializerOptions();
          jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

          var newIngressJsonRaw = JsonSerializer.Serialize(ingress, jsonSerializerOptions);
          var newIngressJsonDoc = JsonDocument.Parse(newIngressJsonRaw);
          return Kubewarden.MutateRequest(newIngressJsonDoc);
        }
        return Kubewarden.RejectRequest("Ingress defaultBackend must not be set", null, null, null);
      }
      return Kubewarden.AcceptRequest();
    }
    else
    {
      return Kubewarden.RejectRequest("Cannot convert request.Object to Ingress", 400, null, null);
    }
  }


}
