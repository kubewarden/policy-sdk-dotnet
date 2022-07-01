namespace KubewardenPolicySDK;

using System.Text.Json;
using System.Text.Json.Serialization;

#pragma warning disable CS8618
public class ValidationRequest
{
  [JsonPropertyName("settings")]
  public JsonDocument Settings { get; set; }

  [JsonPropertyName("request")]
  public KubernetesAdmissionRequest Request { get; set; }
}

public class KubernetesAdmissionRequest
{
  [JsonPropertyName("uid")]
  public string Uid { get; set; }

  [JsonPropertyName("kind")]
  public GroupVersionKind Kind { get; set; }

  [JsonPropertyName("resource")]
  public GroupVersionResource Resource { get; set; }

  [JsonPropertyName("subResource")]
  public string? SubResource { get; set; }

  [JsonPropertyName("requestKind")]
  public GroupVersionKind? RequestKind { get; set; }

  [JsonPropertyName("requestResource")]
  public GroupVersionResource? RequestResource { get; set; }

  [JsonPropertyName("requestSubResource")]
  public string? RequestSubResource { get; set; }

  [JsonPropertyName("name")]
  public string Name { get; set; }

  [JsonPropertyName("namespace")]
  public string Namespace { get; set; }

  [JsonPropertyName("operation")]
  public string Operation { get; set; }

  [JsonPropertyName("userInfo")]
  public UserInfo UserInfo { get; set; }

  [JsonPropertyName("object")]
  public JsonDocument? Object { get; set; }

  [JsonPropertyName("oldObject")]
  public JsonDocument? OldObject { get; set; }

  [JsonPropertyName("dryRun")]
  public bool? DryRun { get; set; }

  [JsonPropertyName("options")]
  public Dictionary<String, JsonDocument>? Options { get; set; }
}

public class GroupVersionKind
{
  [JsonPropertyName("group")]
  public string Group { get; set; }

  [JsonPropertyName("version")]
  public string Version { get; set; }

  [JsonPropertyName("kind")]
  public string Kind { get; set; }
}

public class GroupVersionResource
{
  [JsonPropertyName("group")]
  public string Group { get; set; }

  [JsonPropertyName("version")]
  public string Version { get; set; }

  [JsonPropertyName("kind")]
  public string Kind { get; set; }
}

public class UserInfo
{
  [JsonPropertyName("username")]
  public string Username { get; set; }

  [JsonPropertyName("uid")]
  public string Uid { get; set; }

  [JsonPropertyName("groups")]
  public List<string> Groups { get; set; }

  [JsonPropertyName("extra")]
  public Dictionary<string, JsonDocument> Extra { get; set; }
}
#pragma warning restore CS8618