namespace KubewardenPolicySDK;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Class <c>ValidationResponse</c> describes a validation response object
/// each policy must return.
/// </summary>
public class ValidationResponse
{
  [JsonPropertyName("accepted")]
  public bool Accepted { get; set; }

  [JsonPropertyName("message")]
  public string? Message { get; set; }

  [JsonPropertyName("code")]
  public int? Code { get; set; }

  [JsonPropertyName("mutated_object")]
  public JsonDocument? MutatedObject { get; set; }

  [JsonPropertyName("audit_annotations")]
  public Dictionary<String, String>? AuditAnnotations { get; set; }

  [JsonPropertyName("warnings")]
  public List<String>? Warnings { get; set; }
}

public class SettingsValidationResponse
{
  [JsonPropertyName("valid")]
  public bool Valid { get; set; }

  [JsonPropertyName("message")]
  public string? Message { get; set; }
}