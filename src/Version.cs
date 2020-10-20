using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace Shields
{
    public static class Version
    {
        const string defaultFeed = "https://pkg.kzu.io/index.json";
        static readonly PackageIdentity nullPackage = new PackageIdentity("null", new NuGet.Versioning.NuGetVersion("0.0.0-null"));

        [FunctionName("v")]
        public static Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v/{id}")] HttpRequestMessage req, string id)
            => Run(req, id, false);

        [FunctionName("vpre")]
        public static Task<HttpResponseMessage> RunPre(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "vpre/{id}/{label?}")] HttpRequestMessage req, string id, string? label)
            => Run(req, id, true, label);

        static async Task<HttpResponseMessage> Run(HttpRequestMessage req, string packageId, bool includePrerelease, string? prereleaseLabel = null)
        {
            var feedUrl = defaultFeed;
            if (!string.IsNullOrEmpty(req.RequestUri.Query))
            {
                var qs = req.RequestUri.ParseQueryString();
                feedUrl = qs["feed"] ?? qs["f"] ?? defaultFeed;
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

            return req.CreateCachedResponse(HttpStatusCode.OK, content);
        }

        static async Task<PackageIdentity> GetPackage(string packageId, string feedUrl, bool includePrerelease = false, string? prereleaseLabel = null)
        {
            var cache = MemoryCache.Default;
            var package = cache["v|" + packageId] as PackageIdentity;
            if (package == null)
            {
                var providers = Repository.Provider.GetCoreV3();
                var source = new PackageSource(feedUrl);
                var repo = new SourceRepository(source, providers);
                var resource = await repo.GetResourceAsync<PackageMetadataResource>();
                var metadata = await resource.GetMetadataAsync(packageId, includePrerelease, false, NuGet.Common.NullLogger.Instance, CancellationToken.None);

                IEnumerable<PackageIdentity> query = metadata.Select(m => m.Identity).OrderByDescending(m => m.Version);
                if (!string.IsNullOrEmpty(prereleaseLabel))
                    query = query.Where(m => m.Version.IsPrerelease && m.Version.ReleaseLabels.Any(l => l.StartsWith(prereleaseLabel)));

                package = query.FirstOrDefault() ?? nullPackage;

                var policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1),
                };

                cache.Set("v|" + packageId, package, policy);
            }

            return package;
        }
    }
}
