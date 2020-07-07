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

    public interface IObjectInformationSource
    {
        IAsyncEnumerable<ObjectInformation> GetObjects();
    }

    public class FileBasedObjectInformationSource : IObjectInformationSource
    {
        private readonly string _path;
        
        private static async IAsyncEnumerable<ObjectInformation> GetObjectsFromDirectory(
            string path,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            foreach (var file in Directory.GetFiles(path))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                using var reader = File.OpenText(file);
                yield return new ObjectInformation(file, await reader.ReadToEndAsync());
            }
        }


        public FileBasedObjectInformationSource(string path)
        {
            _path = path;
        }

        public IAsyncEnumerable<ObjectInformation> GetObjects() => GetObjectsFromDirectory(_path);
    }

    public class Program
    {
        
        static async Task Main(string[] args)
        {
            var path = args[0];
            var cancellation = new CancellationTokenSource();
            IObjectInformationSource objectSource = new FileBasedObjectInformationSource(path);
            await foreach (var objectInformation in objectSource.GetObjects().WithCancellation(cancellation.Token))
            {
                Console.WriteLine($"File: {objectInformation.URI}");
                Console.WriteLine($"Content: {objectInformation.Content}");
            }
        }
    }
}