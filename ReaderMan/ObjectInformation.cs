using System;

namespace ReaderMan
{
    public class ObjectInformation<T>
    {
        public readonly Uri Uri;

        public readonly T Content;

        public ObjectInformation(Uri uri, T content)
        {
            Uri = uri;
            Content = content;
        }
    }
}