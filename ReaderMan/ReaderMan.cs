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
            Func<TConfig, Task<IEnumerable<Uri>>> objectSourceFunc,
            Func<Uri, Task<TResult>> streamParserFunc,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            foreach (var uri in (await objectSourceFunc(config)))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                yield return new ObjectInformation<TResult>(uri, await streamParserFunc(uri));
            }
        }
    }
}