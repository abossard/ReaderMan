using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReaderMan.Local
{
    public class FileBasedObjectInformationSource : IObjectInformationSource<string>
    {
        private readonly string _path;

        public FileBasedObjectInformationSource(string path)
        {
            _path = path;
        }

        private static Task<IEnumerable<Uri>> GetFilesFromDirectory(string path) =>
            Task.FromResult(Directory.GetFiles(path).Select(file => new Uri(file)));

        private static async Task<string> ReadLocalFileUri(Uri localFile)
        {
            using var reader = File.OpenText(localFile.AbsolutePath);
            return await reader.ReadToEndAsync();
        }

        public IAsyncEnumerable<ObjectInformation<string>> GetObjects() =>
            ReaderMan.GetObjectsComposer(_path, GetFilesFromDirectory, ReadLocalFileUri);
    }
}