using System.Collections.Generic;

namespace Configs
{
    public interface IContainerResources
    {  
        string Name { get; }
        List<KeyValuePair<string, object>> GetValues();
    }
}