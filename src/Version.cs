using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace Shields;

public class Version(IMemoryCache cache)
{
    const string defaultFeed = "https://pkg.kzu.app/index.json";
    static readonly PackageIdentity nullPackage = new("null", new NuGet.Versioning.NuGetVersion("0.0.0-null"));

    [Function("v")]
    public Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v/{id}")] HttpRequestData req, string id)
        => Run(req, id, false);

    [Function("vpre")]
    public Task<HttpResponseData> RunPre(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "vpre/{id}/{label?}")] HttpRequestData req, string id, string? label)
        => Run(req, id, true, label);

    async Task<HttpResponseData> Run(HttpRequestData req, string packageId, bool includePrerelease, string? prereleaseLabel = null)
    {
        var feedUrl = defaultFeed;
        if (req.Query.Count > 0)
        {
            feedUrl = req.Query["feed"] ?? req.Query["f"] ?? defaultFeed;
        }

        if (!feedUrl.StartsWith("http://") && !feedUrl.StartsWith("https://"))
            feedUrl = "https://" + feedUrl;

        var package = await GetPackage(packageId, feedUrl, includePrerelease, prereleaseLabel);
        var content = package == nullPackage ?
            new
            {
                schemaVersion = 1,
                label = packageId + ".404",
                message = "NotFound",
            } :
            new
            {
                schemaVersion = 1,
                label = packageId,
                message = package.Version.ToNormalizedString(),
            };

        return await req.CreateCachedResponseDataAsync(HttpStatusCode.OK, content);
    }

    async Task<PackageIdentity> GetPackage(string packageId, string feedUrl, bool includePrerelease = false, string? prereleaseLabel = null)
    {
        var key = includePrerelease ?
            feedUrl + "|vpre|" + packageId + "|" + (prereleaseLabel ?? "") :
            feedUrl + "|v|" + packageId;

        if (!cache.TryGetValue<PackageIdentity>(key, out var package) || package == null)
        {
            var providers = Repository.Provider.GetCoreV3();
            var source = new PackageSource(feedUrl);
            var repo = new SourceRepository(source, providers);
            using var cacheContext = new SourceCacheContext();
            var resource = await repo.GetResourceAsync<PackageMetadataResource>();
            var metadata = await resource.GetMetadataAsync(packageId, includePrerelease, includeUnlisted: false, 
                sourceCacheContext: cacheContext,
                NuGet.Common.NullLogger.Instance, CancellationToken.None);

            IEnumerable<PackageIdentity> query = metadata.Select(m => m.Identity).OrderByDescending(m => m.Version);
            if (!string.IsNullOrEmpty(prereleaseLabel))
                query = query.Where(m => m.Version.IsPrerelease && m.Version.ReleaseLabels.Any(l => l.StartsWith(prereleaseLabel)));

            package = query.FirstOrDefault() ?? nullPackage;
            cache.Set(key, package, DateTimeOffset.Now.AddMinutes(1));
        }

        return package;
    }
}
