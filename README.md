# shields endpoints

Custom endpoints for custom badges using https://shields.io/endpoint. 

Main usage is to use as your shields badge the URL `https://img.shields.io/endpoint?url=https://shields.kzu.io/`  followed by one of the supported endpoints below.

# nuget version endpoints

The built-in [shields.io](https://shields.io/category/version) support for nuget versions only works with the nuget.org repository. We provide support for arbitrary feeds as follows:

```
[v|vpre]/[package id]?feed=[v3 feed]
```

You can also abbreviate `feed` as just `f`. 

The following examples assume the following feed passed in as the last argument (unless overriden): `?f=pkg.kzu.io/index.json` which is my [sleet](https://github.com/emgarten/Sleet) feed I use for CI packages:

| Badge                                                                                                                                                 | Arguments                                                                         |
| ----------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/Microsoft.AspNetCore.Mvc?f=dotnet.myget.org/F/aspnetcore-dev/api/v3/index.json) | /vpre/Microsoft.AspNetCore.Mvc?f=dotnet.myget.org/F/aspnetcore-dev/api/v3/index.json |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.io/v/dotnet-file?f=pkg.kzu.io/index.json&color=blue)                                 | /v/dotnet-file&color=blue                                                         |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/dotnet-config?f=pkg.kzu.io/index.json)                                       | /vpre/dotnet-config                                                               |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/Stunts?f=pkg.kzu.io/index.json)                                              | /vpre/Stunts                                                                      |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/Stunts/main?f=pkg.kzu.io/index.json)                                         | /vpre/Stunts/main                                                                 |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/Stunts/pr10?f=pkg.kzu.io/index.json&color=green)                             | /vpre/Stunts/pr10&color=green                                                     |
| ![badge](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/Stunts/main&labelColor=yellow&logoColor=004880&logo=NuGet&style=flat-square) | /vpre/Stunts/main&labelColor=yellow&logoColor=004880&logo=NuGet&style=flat-square |