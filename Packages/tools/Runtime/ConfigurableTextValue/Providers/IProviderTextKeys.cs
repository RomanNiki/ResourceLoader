using System.Collections.Generic;

namespace Openmygame.ConfigurableTextValue.Providers
{
    public interface IProviderTextKeys
    {
        IEnumerable<string> GetKeys();
    }
}