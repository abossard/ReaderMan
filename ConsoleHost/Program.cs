﻿using System;
using System.Threading;
using System.Threading.Tasks;
using ReaderMan;
using ReaderMan.Local;

// https://docs.microsoft.com/en-us/archive/msdn-magazine/2019/november/csharp-iterating-with-async-enumerables-in-csharp-8
namespace ConsoleHost
{
    public static class Program
    {
        
        public static async Task Main(string[] args)
        {
            var path = args[0];
            var cancellation = new CancellationTokenSource();
            IObjectInformationSource<string> objectSource = new FileBasedObjectInformationSource(path);
            await foreach (var objectInformation in objectSource.GetObjects().WithCancellation(cancellation.Token))
            {
                Console.WriteLine($"File: {objectInformation.Uri}");
                Console.WriteLine($"Content: {objectInformation.Content}");
            }
        }
    }
}