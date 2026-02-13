using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Capabilities;

namespace Net;

/// <summary>
/// Network capabilities using the Kubewarden host
/// </summary>
public class NetworkOperations : INetworkOperations
{
    /// <summary>
    /// Looks up the addresses for a given hostname via DNS.
    /// </summary>
    /// <param name="host">The host object with WapcClient capabilities</param>
    /// <param name="hostname">The hostname to look up</param>
    /// <returns>List of IP addresses associated with the hostname</returns>
    public List<string> LookupHost(Host host, string hostname)
    {
        if (host == null || host.Client == null)
        {
            throw new ArgumentNullException(nameof(host), "Host or Host.Client cannot be null");
        }

        if (string.IsNullOrEmpty(hostname))
        {
            throw new ArgumentNullException(nameof(hostname), "Hostname cannot be null or empty");
        }

        // Build request payload - serialize the hostname to JSON using JsonContext
        byte[] payload;
        try
        {
            payload = JsonSerializer.SerializeToUtf8Bytes(hostname, NetJsonContext.Default.String);
        }
        catch (Exception ex)
        {
            throw new Exception($"Cannot serialize host to JSON: {ex.Message}");
        }

        // Perform host callback
        byte[] responsePayload;
        try
        {
            responsePayload = host.Client.HostCall("kubewarden", "net", "v1/dns_lookup_host", payload);
        }
        catch (Exception ex)
        {
            throw new Exception($"Host call failed: {ex.Message}");
        }

        // Deserialize the response using JsonContext
        try
        {
            var response = JsonSerializer.Deserialize(responsePayload, NetJsonContext.Default.LookupHostResponse);

            // Ensure we don't return null
            return response?.Ips ?? new List<string>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Cannot deserialize response: {ex.Message}");
        }
    }
}