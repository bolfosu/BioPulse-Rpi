using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace LogicLayer.Proxies
{
    [DBusInterface("org.freedesktop.DBus.Properties")]
    public interface IProperties : IDBusObject
    {
        Task<IDictionary<string, object>> GetAllAsync(string interfaceName);
        Task<object> GetAsync(string interfaceName, string propertyName);
        Task SetAsync(string interfaceName, string propertyName, object value);
    }
}
