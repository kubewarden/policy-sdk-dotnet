
using System.Text.Json.Serialization;
namespace Net;


/// <summary>
/// A JSON serialization context for handling types like string and LookupHostResponse.
/// </summary>
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(LookupHostResponse))]
public partial class NetJsonContext : JsonSerializerContext
{
}