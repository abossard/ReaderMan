using System.Collections.Generic;

namespace ReaderMan
{
    
    public interface IObjectInformationSource<T>
    {
        IAsyncEnumerable<ObjectInformation<T>> GetObjects();
    }
}