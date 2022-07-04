# Kubewarden .NET Policy SDK

>⚠️ **Warning: experimental** ⚠️
>
> This code leverages [`dot-net-wasi-sdk`](https://github.com/SteveSandersonMS/dotnet-wasi-sdk),
> which is currently marked as experimental.
>
> It also requires usage of .NET 7, which is currently in preview.

This library provides a SDK that can be used to write [Kubewarden Policies](https://kubewarden.io)
using the C# programming language.

This is a first iteration of the SDK, it can be used to create both validating
and mutating policies.

However, the following host capabilities are not exposed yet:

* [ ] Logging
* [ ] [Signature verification](https://docs.kubewarden.io/writing-policies/spec/host-capabilities/signature-verifier-policies)
* [ ] [Container registry operations](https://docs.kubewarden.io/writing-policies/spec/host-capabilities/container-registry)
* [ ] [Network capabilities](https://docs.kubewarden.io/writing-policies/spec/host-capabilities/net)

## Requirements

The code requires .NET 7, which is currently (as of July 2022) in preview mode.
Executing `dotnet --version` should return `7.0.100-preview.4` or later.

## Policy quickstart

Start by creating a Console application:

```console
dotnet new console -o MyFirstKubewardenPolicy
cd MyFirstKubewardenPolicy
dotnet add package Wasi.Sdk --prerelease
dotnet add package Kubewarden.Sdk
```

Edit the `Program.cs` file and replace its contents to match the following ones:

```cs
using WapcGuest;
using KubewardenPolicySDK;

namespace Policy;

public class IngressPolicy
{
  public static void Main()
  {
    var wapc = new Wapc();
    wapc.RegisterFunction("protocol_version", Kubewarden.ProtocolVersionGuest);
    wapc.RegisterFunction("validate", Validate);
    wapc.RegisterFunction("validate_settings", SettingsValidate);
  }

  static byte[] Validate(byte[] payload)
  {
    return Kubewarden.AcceptRequest();
  }

  static byte[] SettingsValidate(byte[] payload)
  {
    return Kubewarden.AcceptSettings();
  }
}
```

Finally, build the policy in this way:

```console
dotnet build
```

This will produce a `.wasm` file under the `bin/Debug` directory.
The policy can now be run using [`kwctl`](https://github.com/kubewarden/kwctl/).

For a more complex example, checkout the `examples` directory.

## Contribute

The author of this code is not a .NET expert, patches are welcome to improve the
code quality and to make it more idiomatic.
