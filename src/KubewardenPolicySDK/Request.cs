namespace KubewardenPolicySDK;

using System.Text.Json;
using System.Text.Json.Serialization;

#pragma warning disable CS8618
/// <summary>
/// The data used by policies during their <c>Validate</c> function.
/// </summary>
public class ValidationRequest
{
  /// <summary>
  /// The policy settings
  /// </summary>
  [JsonPropertyName("settings")]
  public JsonDocument Settings { get; set; }

  /// <summary>
  /// Kubernetes' AdmissionReview
  /// </summary>
  /// <seealso href="https://kubernetes.io/docs/reference/access-authn-authz/extensible-admission-controllers" />
  [JsonPropertyName("request")]
  public KubernetesAdmissionRequest Request { get; set; }
}

/// <summary>
/// Representation of a Kubernetes Admission request
/// </summary>
public class KubernetesAdmissionRequest
{
  /// <summary>
  /// UID is an identifier for the individual request/response. It allows us to distinguish instances of requests which are
  /// otherwise identical (parallel requests, requests when earlier requests did not modify etc)
  /// The UID is meant to track the round trip (request/response) between the KAS and the WebHook, not the user request.
  /// It is suitable for correlating log entries between the webhook and apiserver, for either auditing or debugging.
  /// </summary>
  [JsonPropertyName("uid")]
  public string Uid { get; set; }

  /// <summary>
  /// Kind is the fully-qualified type of object being submitted (for example, v1.Pod or autoscaling.v1.Scale)
  /// </summary>
  [JsonPropertyName("kind")]
  public GroupVersionKind Kind { get; set; }

  /// <summary>
  /// Resource is the fully-qualified resource being requested (for example, v1.pods)
  /// </summary>
  [JsonPropertyName("resource")]
  public GroupVersionResource Resource { get; set; }


  /// <summary>
  /// SubResource is the subresource being requested, if any (for example, "status" or "scale")
  /// </summary>
  [JsonPropertyName("subResource")]
  public string? SubResource { get; set; }

  /// <summary>
  /// RequestKind is the fully-qualified type of the original API request (for example, v1.Pod or autoscaling.v1.Scale).
  /// If this is specified and differs from the value in "kind", an equivalent match and conversion was performed.
  ///
  /// For example, if deployments can be modified via apps/v1 and apps/v1beta1, and a webhook registered a rule of
  /// `apiGroups:["apps"], apiVersions:["v1"], resources: ["deployments"]` and `matchPolicy: Equivalent`,
  /// an API request to apps/v1beta1 deployments would be converted and sent to the webhook
  /// with `kind: {group:"apps", version:"v1", kind:"Deployment"}` (matching the rule the webhook registered for),
  /// and `requestKind: {group:"apps", version:"v1beta1", kind:"Deployment"}` (indicating the kind of the original API request).
  ///
  /// See documentation for the "matchPolicy" field in the webhook configuration type for more details.
  /// </summary>
  [JsonPropertyName("requestKind")]
  public GroupVersionKind? RequestKind { get; set; }

  /// <summary>
  /// RequestResource is the fully-qualified resource of the original API request (for example, v1.pods).
  /// If this is specified and differs from the value in "resource", an equivalent match and conversion was performed.
  ///
  /// For example, if deployments can be modified via apps/v1 and apps/v1beta1, and a webhook registered a rule of
  /// `apiGroups:["apps"], apiVersions:["v1"], resources: ["deployments"]` and `matchPolicy: Equivalent`,
  /// an API request to apps/v1beta1 deployments would be converted and sent to the webhook
  /// with `resource: {group:"apps", version:"v1", resource:"deployments"}` (matching the resource the webhook registered for),
  /// and `requestResource: {group:"apps", version:"v1beta1", resource:"deployments"}` (indicating the resource of the original API request).
  ///
  /// See documentation for the "matchPolicy" field in the webhook configuration type.
  /// </summary>
  [JsonPropertyName("requestResource")]
  public GroupVersionResource? RequestResource { get; set; }

  /// <summary>
  /// RequestSubResource is the name of the subresource of the original API request, if any (for example, "status" or "scale")
  /// If this is specified and differs from the value in "subResource", an equivalent match and conversion was performed.
  /// See documentation for the "matchPolicy" field in the webhook configuration type.
  /// </summary>
  [JsonPropertyName("requestSubResource")]
  public string? RequestSubResource { get; set; }

  /// <summary>
  /// Name is the name of the object as presented in the request.  On a CREATE operation, the client may omit name and
  /// rely on the server to generate the name.  If that is the case, this field will contain an empty string.
  /// </summary>
  [JsonPropertyName("name")]
  public string Name { get; set; }

  /// <summary>
  /// Namespace is the namespace associated with the request (if any).
  /// </summary>
  [JsonPropertyName("namespace")]
  public string Namespace { get; set; }

  /// <summary>
  /// Operation is the operation being performed. This may be different than the operation
  /// requested. e.g. a patch can result in either a CREATE or UPDATE Operation.
  /// </summary>
  [JsonPropertyName("operation")]
  public string Operation { get; set; }

  /// <summary>
  /// UserInfo is information about the requesting user
  /// </summary>
  [JsonPropertyName("userInfo")]
  public UserInfo UserInfo { get; set; }

  /// <summary>
  /// Object is the object from the incoming request.
  /// </summary>
  [JsonPropertyName("object")]
  public JsonDocument? Object { get; set; }

  /// <summary>
  /// OldObject is the existing object. Only populated for DELETE and UPDATE requests.
  /// </summary>
  [JsonPropertyName("oldObject")]
  public JsonDocument? OldObject { get; set; }

  /// <summary>
  /// DryRun indicates that modifications will definitely not be persisted for this request.
  /// Defaults to false.
  /// </summary>
  [JsonPropertyName("dryRun")]
  public bool? DryRun { get; set; }

  /// <summary>
  /// Options is the operation option structure of the operation being performed.
  /// e.g. `meta.k8s.io/v1.DeleteOptions` or `meta.k8s.io/v1.CreateOptions`. This may be
  /// different than the options the caller provided. e.g. for a patch request the performed
  /// Operation might be a CREATE, in which case the Options will a
  /// `meta.k8s.io/v1.CreateOptions` even though the caller provided `meta.k8s.io/v1.PatchOptions`.
  /// </summary>
  [JsonPropertyName("options")]
  public Dictionary<String, JsonDocument>? Options { get; set; }
}


/// <summary>
/// GroupVersionKind unambiguously identifies a kind
/// </summary>
public class GroupVersionKind
{
  /// <summary>
  /// Object group name
  /// </summary>
  [JsonPropertyName("group")]
  public string Group { get; set; }

  /// <summary>
  /// Object version
  /// </summary>
  [JsonPropertyName("version")]
  public string Version { get; set; }

  /// <summary>
  /// Object type
  /// </summary>
  [JsonPropertyName("kind")]
  public string Kind { get; set; }
}

/// <summary>
/// GroupVersionKind unambiguously identifies a resource
/// </summary>
public class GroupVersionResource
{
  /// <summary>
  /// Object group name
  /// </summary>
  [JsonPropertyName("group")]
  public string Group { get; set; }

  /// <summary>
  /// Object version
  /// </summary>
  [JsonPropertyName("version")]
  public string Version { get; set; }

  /// <summary>
  /// Object type
  /// </summary>
  [JsonPropertyName("kind")]
  public string Kind { get; set; }
}

/// <summary>
/// UserInfo holds information about the user who made the request
/// </summary>
public class UserInfo
{
  /// <summary>
  /// The name that uniquely identifies this user among all active users.
  /// </summary>
  [JsonPropertyName("username")]
  public string Username { get; set; }

  /// <summary>
  /// A unique value that identifies this user across time. If this user is
  /// deleted and another user by the same name is added, they will have
  /// different UIDs.
  /// </summary>
  [JsonPropertyName("uid")]
  public string Uid { get; set; }

  /// <summary>
  /// The names of groups this user is a part of.
  /// </summary>
  [JsonPropertyName("groups")]
  public List<string> Groups { get; set; }

  /// <summary>
  /// Any additional information provided by the authenticator.
  /// </summary>
  [JsonPropertyName("extra")]
  public Dictionary<string, JsonDocument> Extra { get; set; }
}
#pragma warning restore CS8618
