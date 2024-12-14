using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace LogicLayer.Proxies
{
    [DBusInterface("org.freedesktop.DBus.ObjectManager")]
    public interface IObjectManager : IDBusObject
    {
        Task<IDictionary<ObjectPath, IDictionary<string, IDictionary<string, object>>>> GetManagedObjectsAsync();
    }
}
