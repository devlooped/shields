using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using NuGet.Configuration;
using NuGet.Protocol;
using System.Threading;
using System.Linq;
using NuGet.Packaging.Core;
using System.Collections.Generic;

namespace Shields
{
    public static class Version
    {
        const string defaultFeed = "https://pkg.kzu.io/index.json";

        [FunctionName("version")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "version/{id}/{label?}")] HttpRequest req,
            ILogger log, string id, string? label)
        {
            var feedUrl = req.Query.TryGetValue("feed", out var feed) ?
                feed.FirstOrDefault() ?? defaultFeed :
                req.Query.TryGetValue("f", out var f) ?
                f.FirstOrDefault() ?? defaultFeed :
                defaultFeed;

            if (!feedUrl.StartsWith("http://") || !feedUrl.StartsWith("https://"))
                feedUrl = "https://" + feedUrl;

            var providers = Repository.Provider.GetCoreV3();
            var source = new PackageSource(feedUrl);
            var repo = new SourceRepository(source, providers);
            var resource = await repo.GetResourceAsync<PackageMetadataResource>();
            var metadata = await resource.GetMetadataAsync(id, !string.IsNullOrEmpty(label), false, NuGet.Common.NullLogger.Instance, CancellationToken.None);

            // 3.1.0 is already stable and we verified we work with it
            // Older versions are guaranteed to not change either, so we 
            // can rely on it working too, since this test passed at some 
            // point too.
            IEnumerable<PackageIdentity> query = metadata.Select(m => m.Identity).OrderByDescending(m => m.Version);

            if (!string.IsNullOrEmpty(label))
                query = query.Where(m => m.Version.IsPrerelease && m.Version.ReleaseLabels.Any(l => l.StartsWith(label)));

            var package = query.FirstOrDefault();

            if (package == null)
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new
                    {
                        schemaVersion = 1,
                        label = id + ".404",
                        message = "NotFound",
                    }),
                    ContentType = "application/json",
                    StatusCode = 200
                };

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(new
                {
                    schemaVersion = 1,
                    label = id,
                    message = package.Version.ToNormalizedString(),
                }),
                ContentType = "application/json",
                StatusCode = 200
            };
        }
    }
}
