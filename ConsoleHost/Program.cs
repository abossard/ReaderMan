using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
///
/// https://docs.microsoft.com/en-us/archive/msdn-magazine/2019/november/csharp-iterating-with-async-enumerables-in-csharp-8
namespace ConsoleHost
{
    public class ObjectInformation
    {
        public readonly string URI;

        public readonly string Content;

        public ObjectInformation(string uri, string content)
        {
            this.URI = uri;
            this.Content = content;
        }
    }

    class Program
    {
        private static async IAsyncEnumerable<ObjectInformation> GetObjectsFromDirectory(
            string path,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            foreach (string file in Directory.GetFiles(path))
            {
                using (var reader = File.OpenText(file))
                {
                    yield return new ObjectInformation(file, await reader.ReadToEndAsync());
                }
            }
        }

        static async Task Main(string[] args)
        {
            var path = args[0];
            var cancellation = new CancellationTokenSource();
            await foreach (var objectInformation in GetObjectsFromDirectory(path).WithCancellation(cancellation.Token))
            {
                Console.WriteLine($"File: {objectInformation.URI}");
                Console.WriteLine($"Content: {objectInformation.Content}");
            }
        }
    }
}