using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace LogicLayer.Proxies
{
    [DBusInterface("org.bluez.Adapter1")]
    public interface IAdapter : IDBusObject
    {
        Task StartDiscoveryAsync();
        Task StopDiscoveryAsync();
        Task<IDictionary<string, object>> GetAllAsync();
    }
}
