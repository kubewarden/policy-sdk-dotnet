namespace KubewardenPolicySDK;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Class <c>ValidationResponse</c> describes a validation response object
/// each policy must return.
/// </summary>
public class ValidationResponse
{

  /// <summary>
  /// True if the request has been accepted, false otherwise
  /// </summary>
  [JsonPropertyName("accepted")]
  public bool Accepted { get; set; }

  /// <summary>
  /// Message shown to the user when the request is rejected
  /// </summary>
  [JsonPropertyName("message")]
  public string? Message { get; set; }

  /// <summary>
  /// Code shown to the user when the request is rejected
  /// </summary>
  [JsonPropertyName("code")]
  public int? Code { get; set; }

  /// <summary>
  /// Mutated Object - used only by mutation policies
  /// </summary>
  [JsonPropertyName("mutated_object")]
  public JsonDocument? MutatedObject { get; set; }

  /// <summary>
  /// AuditAnnotations is an unstructured key value map set by remote admission controller (e.g. error=image-blacklisted).
  /// MutatingAdmissionWebhook and ValidatingAdmissionWebhook admission controller will prefix the keys with
  /// admission webhook name (e.g. imagepolicy.example.com/error=image-blacklisted). AuditAnnotations will be provided by
  /// the admission webhook to add additional context to the audit log for this request.
  /// </summary>
  [JsonPropertyName("audit_annotations")]
  public Dictionary<String, String>? AuditAnnotations { get; set; }

  /// <summary>
  /// warnings is a list of warning messages to return to the requesting API client.
  /// Warning messages describe a problem the client making the API request should correct or be aware of.
  /// Limit warnings to 120 characters if possible.
  /// Warnings over 256 characters and large numbers of warnings may be truncated.
  /// </summary>
  [JsonPropertyName("warnings")]
  public List<String>? Warnings { get; set; }
}

/// <summary>
/// A SettingsValidationResponse object holds the outcome of settings
/// validation.
/// </summary>
public class SettingsValidationResponse
{

  /// <summary>
  /// True if the settings are valid
  /// </summary>
  [JsonPropertyName("valid")]
  public bool Valid { get; set; }

  /// <summary>
  /// Message shown to the user when the settings are not valid
  /// </summary>
  [JsonPropertyName("message")]
  public string? Message { get; set; }
}
