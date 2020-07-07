using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderMan
{
    public static class ReaderMan
    {
        public static async IAsyncEnumerable<ObjectInformation<TResult>> GetObjectsComposer<TConfig, TResult>(
            TConfig config,
            Func<TConfig, Task<IEnumerable<Uri>>> listPackageUrisFromConfigFunc,
            Func<Uri, IAsyncEnumerable<Tuple<string, TResult>>> readPackageIntoContent,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            foreach (var uri in await listPackageUrisFromConfigFunc(config))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                await foreach (var (name, content) in readPackageIntoContent(uri))
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    yield return new ObjectInformation<TResult>(new Uri(uri + "/" + name), content);                    
                }
            }
        }
    }
}