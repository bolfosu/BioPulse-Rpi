using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace LogicLayer.Proxies
{
    [DBusInterface("org.bluez.GattCharacteristic1")]
    public interface IGattCharacteristic : IDBusObject
    {
        Task<byte[]> ReadValueAsync(IDictionary<string, object> options);
        Task WriteValueAsync(byte[] value, IDictionary<string, object> options);
    }
}
