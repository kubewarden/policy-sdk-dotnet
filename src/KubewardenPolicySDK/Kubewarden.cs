﻿namespace KubewardenPolicySDK;

using k8s.Models;
using k8s;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WapcGuest;
using System.Reflection;

/// <summary>
/// Set of helper methods used to write Kubewarden policies.
/// </summary>
public class Kubewarden
{
  /// <summary>
  /// Create an acceptance response
  /// </summary>
  public static byte[] AcceptRequest()
  {
    var response = new ValidationResponse
    {
      Accepted = true,
    };

    return JsonSerializer.SerializeToUtf8Bytes(response);
  }

  /// <summary>
  /// Create an acceptance response that mutates the original object
  /// </summary>
  /// <param name="mutatedObject">the mutated object</param>
  public static byte[] MutateRequest(JsonDocument mutatedObject)
  {
    var response = new ValidationResponse
    {
      Accepted = true,
      MutatedObject = mutatedObject,
    };
    var jsonSerializerOptions = new JsonSerializerOptions();
    jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

    return JsonSerializer.SerializeToUtf8Bytes(response, jsonSerializerOptions);
  }

  /// <summary>
  /// Create a rejection response
  /// </summary>
  /// Create a rejection response
  /// <param name="message">message shown to the user</param>
  /// <param name="code">code shown to the user</param>
  /// <param name="auditAnnotations">an unstructured key value map set by remote admission controller (e.g. error=image-blacklisted). MutatingAdmissionWebhook and ValidatingAdmissionWebhook admission controller will prefix the keys with admission webhook name (e.g. imagepolicy.example.com/error=image-blacklisted). AuditAnnotations will be provided by the admission webhook to add additional context to the audit log for this request.</param>
  /// <param name="warnings">a list of warning messages to return to the requesting API client. Warning messages describe a problem the client making the API request should correct or be aware of. Limit warnings to 120 characters if possible. Warnings over 256 characters and large numbers of warnings may be truncated.</param>
  public static byte[] RejectRequest(
    string? message,
    int? code,
    Dictionary<string, string>? auditAnnotations,
    List<string>? warnings
  )
  {
    var response = new ValidationResponse
    {
      Accepted = false,
      Message = message,
      Code = code,
      AuditAnnotations = auditAnnotations,
      Warnings = warnings,
    };

    return JsonSerializer.SerializeToUtf8Bytes(response);
  }

  /// <summary>
  /// Create a settings validation response accepting the user provided values
  /// </summary>
  public static byte[] AcceptSettings()
  {
    var response = new SettingsValidationResponse
    {
      Valid = true,
    };

    return JsonSerializer.SerializeToUtf8Bytes(response);
  }

  /// <summary>
  /// Create a settings validation response that rejects the user provided values
  /// </summary>
  /// <param name="message">message shown to the user</param>
  public static byte[] RejectSettings(string? message)
  {
    var response = new SettingsValidationResponse
    {
      Valid = false,
      Message = message,
    };

    return JsonSerializer.SerializeToUtf8Bytes(response);
  }

  /// <summary>
  /// waPC guest function each Kubewarden policy must provide.
  /// <para>
  /// This function must be registered by each Kubewarden policy. It provides
  /// information to the host about the protocol version used by the policy.
  /// </para>
  /// </summary>
  /// <example>
  /// Each policy should include the following code:
  /// <code>
  /// var wapc = new Wapc();
  /// wapc.RegisterFunction("protocol_version", Kubewarden.ProtocolVersionGuest);
  /// </code>
  /// </example>
  public static byte[] ProtocolVersionGuest(byte[] _payload)
  {
    string protocol_version = "v1";
    return JsonSerializer.SerializeToUtf8Bytes(protocol_version);
  }

    /// <summary>
    /// Invokes the `list_resources` capability of the host
    /// </summary>
    /// <param name="labelSelector"></param>
    /// <param name="fieldSelector"></param>
    public static KubernetesList<T>? ListResourcesAll<T>(string labelSelector, string fieldSelector) where T : class, IKubernetesObject
    {
        var input = JsonSerializer.Serialize(new
        {
            api_version = ExtractAPIVersion<T>(),
            kind = ExtractKubeKind<T>(),
            label_selector = labelSelector,
            field_selector = fieldSelector
        });

        var resp = Wapc.HostCall("kubewarden", "kubernetes", "list_resources_all", Encoding.UTF8.GetBytes(input));
        var responseString = Encoding.UTF8.GetString(resp);
        if (String.IsNullOrEmpty(responseString))
        {
            return null;
        }
        try
        {
            return JsonSerializer.Deserialize<KubernetesList<T>>(responseString);
        }
        catch (JsonException e)
        {
            throw new JsonException($"Error deserializing response: {resp}", e);
        }
    }

    /// <summary>
    /// Invokes the `list_resources_by_namespace` capability of the host
    /// </summary>
    /// <param name="labelSelector"></param>
    /// <param name="fieldSelector"></param>
    /// <param name="k8sNamespace"></param>
    public static KubernetesList<T>? ListResourcesByNamespace<T>(string k8sNamespace, string labelSelector, string fieldSelector) where T : class, IKubernetesObject
    {
        var input = JsonSerializer.Serialize(new
        {
            api_version = ExtractAPIVersion<T>(),
            kind = ExtractKubeKind<T>(),
            label_selector = labelSelector,
            field_selector = fieldSelector,
            @namespace = k8sNamespace
        });

        var resp = Wapc.HostCall("kubewarden", "kubernetes", "list_resources", Encoding.UTF8.GetBytes(input));
        var responseString = Encoding.UTF8.GetString(resp);
        if (String.IsNullOrEmpty(responseString))
        {
            return null;
        }
        try
        {
            return JsonSerializer.Deserialize<KubernetesList<T>>(responseString);
        }
        catch (JsonException e)
        {
            throw new JsonException($"Error deserializing response: {resp}", e);
        }
    }

    /// <summary>
    /// Invokes the `list_resources_by_namespace` capability of the host
    /// </summary>
    /// <param name="k8sNamespace"></param>
    /// <param name="name"></param>
    /// <param name="disable_cache"></param>
    public static T? GetResource<T>(string k8sNamespace, string name, bool disable_cache) where T : class, IKubernetesObject
    {
        var input = JsonSerializer.Serialize(new
        {
            api_version = ExtractAPIVersion<T>(),
            kind = ExtractKubeKind<T>(),
            @namespace = k8sNamespace,
            name = name,
            disable_cache = disable_cache
        });

        var resp = Wapc.HostCall("kubewarden", "kubernetes", "get_resource", Encoding.UTF8.GetBytes(input));
        var responseString = Encoding.UTF8.GetString(resp);
        if (String.IsNullOrEmpty(responseString))
        {
            return null;
        }
        try
        {
            return JsonSerializer.Deserialize<T>(responseString);
        }
        catch (JsonException e)
        {
            throw new JsonException($"Error deserializing response: {resp}", e);
        }
    }

    private static string ExtractKubeKind<T>() where T : IKubernetesObject
    {
        var type = typeof(T);
        var kea = type.GetCustomAttribute<KubernetesEntityAttribute>();
        return kea?.Kind ?? type.Name;
    }

    private static string ExtractAPIVersion<T>() where T : IKubernetesObject
    {
        var type = typeof(T);
        var kea = type.GetCustomAttribute<KubernetesEntityAttribute>();
        return kea?.ApiVersion ?? type.Name;
    }
}

