[![Incubating](https://img.shields.io/badge/status-incubating-orange?style=for-the-badge)](https://github.com/kubewarden/community/blob/main/REPOSITORIES.md#incubating)

# Kubewarden .NET Policy SDK

>⚠️ **Warning: experimental** ⚠️
>
> This code leverages [`dot-net-wasi-sdk`](https://github.com/SteveSandersonMS/dotnet-wasi-sdk),
> which is currently marked as experimental.

This repository provides a SDK that can be used to write [Kubewarden Policies](https://kubewarden.io)
using the C# programming language.

## Requirements

The code requires .NET 7.

## Repository layout

The repository contains the following resources:

  * `src`: contains the source code of the waPC guest library
  * `examples`: contains a demo program and a waPC runtime to run it

## Contribute

The author of this code is not a .NET expert, patches are welcome to improve the
code quality and to make it more idiomatic.
