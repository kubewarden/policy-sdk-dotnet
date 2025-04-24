using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Capabilities;

namespace Net;

/// <summary>
/// Response object for DNS lookup operations
/// </summary>
public class LookupHostResponse
{
    [JsonPropertyName("ips")]
   public List<string> Ips { get; set; } = new List<string>();
}

