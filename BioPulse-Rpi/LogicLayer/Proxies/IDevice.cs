using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace LogicLayer.Proxies
{
    [DBusInterface("org.bluez.Device1")]
    public interface IDevice : IDBusObject
    {
        Task ConnectAsync();
        Task DisconnectAsync();
        Task<IDictionary<string, object>> GetAllAsync();
    }
}
