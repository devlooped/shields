# shields endpoints

Custom endpoints for custom badges using https://shields.io/endpoint. 

Main usage is to use as your shields badge the URL `https://img.shields.io/endpoint?url=https://shields.kzu.app/`  followed by one of the supported endpoints below.

This service complements nicely [sleet](https://github.com/emgarten/Sleet)-powered feeds, but also virtually all CI systems that produce nuget feeds from build artifacts (Azure packaging example below).

# nuget version endpoints

The built-in [shields.io](https://shields.io/category/version) support for nuget versions only works with the nuget.org repository. We provide support for arbitrary feeds as follows:

```
[v|vpre]/[package id]?feed=[v3 feed]
```

You can also abbreviate `feed` as just `f`. 

| Badge                                                                                                                                                 | Url                                                                         |
| ----------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.app/vpre/dotnet-stack?f=pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json) | `https://img.shields.io/endpoint?`<br/>`url=https://shields.kzu.app/vpre/dotnet-stack?`<br/>`f=pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json` |
| ![badge](https://img.shields.io/endpoint?color=68217A&url=https://shields.kzu.app/v/dotnet-format?f=pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json) | `https://img.shields.io/endpoint?color=68217A&`<br/>`url=https://shields.kzu.app/v/dotnet-format?`<br/>`f=pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json` |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.app/v/dotnet-file?f=pkg.kzu.app/index.json&color=blue)                                 | `https://img.shields.io/endpoint?color=blue`<br/>`url=https://shields.kzu.app/v/dotnet-file?`<br/>`f=pkg.kzu.app/index.json`                                                         |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.app/vpre/dotnet-config?f=pkg.kzu.app/index.json)                                       | `https://img.shields.io/endpoint?`<br/>`url=https://shields.kzu.app/vpre/dotnet-config?`<br/>`f=pkg.kzu.app/index.json`                                                               |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.app/vpre/Avatar?f=pkg.kzu.app/index.json)                                              | `https://img.shields.io/endpoint?`<br/>`url=https://shields.kzu.app/vpre/Avatar?`<br/>`f=pkg.kzu.app/index.json`                                                                      |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.app/vpre/Avatar/main?f=pkg.kzu.app/index.json)                                         | `https://img.shields.io/endpoint?`<br/>`url=https://shields.kzu.app/vpre/Avatar/main?`<br/>`f=pkg.kzu.app/index.json`                                                                 |
| ![badge](https://img.shields.io/endpoint?color=green&url=https://shields.kzu.app/vpre/Avatar/pr111?f=pkg.kzu.app/index.json)                             | `https://img.shields.io/endpoint?color=green&`<br/>`url=https://shields.kzu.app/vpre/Avatar/pr111?`<br/>`f=pkg.kzu.app/index.json`                                                     |
| ![badge](https://img.shields.io/endpoint?labelColor=F4BE00&color=black&logoColor=004880&logo=NuGet&style=flat-square&url=https://shields.kzu.app/vpre/Moq/main) | `https://img.shields.io/endpoint?labelColor=F4BE00&`<br/>`color=black&logoColor=004880&logo=NuGet&style=flat-square&`<br/>`url=https://shields.kzu.app/vpre/Moq/main` |

> NOTE: the `?feed=` must be the first querystring argument after the `url=https://shields.kzu.app` argument to `https://img.shields.io/endpoint`. That way, the subsequent query string arguments after the `&` will be interpreted as querystring arguments for shields.io. Alternatively, you can pass all the shields.io arguments *first* and leave the `&url=` as the last argument (which would only include the `?feed=` then), which is probably safest and easiest to remember.
