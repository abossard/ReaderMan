using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ReaderMan.Tests
{
    public class ReaderManTest
    {
        [Fact]
        public async Task GetObjectsComposer__called_with_a_source__adheres_to_the_protocol()
        {
            const string config = "testConfig";
            var packageUriSourceFuncCalls = 0;
            Task<IEnumerable<Uri>> PackageUriSourceFunc(string domain) => Task.FromResult((IEnumerable<Uri>) new[] {new Uri($"https://{domain}/{++packageUriSourceFuncCalls}")});

            var readPackageIntoObjectsCalls = 0;
            async IAsyncEnumerable<(string Name,string Content)> ReadPackageIntoObjects(Uri uri)
            {
                readPackageIntoObjectsCalls++;
                yield return (Name: $"myFile{readPackageIntoObjectsCalls}", Content: await Task.FromResult($"Result {readPackageIntoObjectsCalls}: {uri}"));
            }

            var resultCount = 0;
            await foreach (var result in ReaderMan.GetObjectsComposer(config, PackageUriSourceFunc, ReadPackageIntoObjects))
            {
                resultCount++;
                result.Content.Should().Contain(config.ToLower());
            }
            packageUriSourceFuncCalls.Should().BeGreaterThan(0);
            readPackageIntoObjectsCalls.Should().BeGreaterThan(0);
            resultCount.Should().BeGreaterThan(0);
        }
    }
}