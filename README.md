# shields endpoints

Custom endpoints for custom badges using https://shields.io/endpoint. 

Main usage is to use as your shields badge the URL `https://img.shields.io/endpoint?url=https://shields.kzu.io/`  followed by one of the supported endpoints below.

This service complements nicely [sleet](https://github.com/emgarten/Sleet)-powered feeds, but also virtually all CI systems that produce nuget feeds from build artifacts (Azure packaging example below).

# nuget version endpoints

The built-in [shields.io](https://shields.io/category/version) support for nuget versions only works with the nuget.org repository. We provide support for arbitrary feeds as follows:

```
[v|vpre]/[package id]?feed=[v3 feed]
```

You can also abbreviate `feed` as just `f`. 

| Badge                                                                                                                                                 | Arguments                                                                         |
| ----------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/dotnet-stack?f=pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json) | `/vpre/dotnet-stack?<br/>f=pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json` |
| ![badge](https://img.shields.io/endpoint?&color=68217A&url=https://shields.kzu.io/v/dotnet-format?f=pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json) | `/v/dotnet-format<br/>f=pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json&color=68217A` |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.io/v/dotnet-file?f=pkg.kzu.io/index.json&color=blue)                                 | `/v/dotnet-file&color=blue?f=pkg.kzu.io/index.json`                                                         |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/dotnet-config?f=pkg.kzu.io/index.json)                                       | `/vpre/dotnet-config?f=pkg.kzu.io/index.json`                                                               |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/Avatar?f=pkg.kzu.io/index.json)                                              | `/vpre/Avatar?f=pkg.kzu.io/index.json`                                                                      |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/Avatar/main?f=pkg.kzu.io/index.json)                                         | `/vpre/Avatar/main?f=pkg.kzu.io/index.json`                                                                 |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/Avatar/pr77?f=pkg.kzu.io/index.json&color=green)                             | `/vpre/Avatar/pr77?f=pkg.kzu.io/index.json&color=green`                                                     |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/Moq/main&labelColor=F4BE00&color=black&logoColor=004880&logo=NuGet&style=flat-square) | /vpre/Moq/main?f=pkg.kzu.io/index.json&labelColor=F4BE00&color=black&<br/>logoColor=004880&logo=NuGet&style=flat-square |

> NOTE: the `?feed=` must be the first querystring argument after the `url=https://shields.kzu.io` argument to `https://img.shields.io/endpoint`. That way, the subsequent query string arguments after the `&` will be interpreted as querystring arguments for shields.io. Alternatively, you can pass all the shields.io arguments *first* and leave the `&url=` as the last argument (which would only include the `?feed=` then), which is probably safest and easiest to remember.
