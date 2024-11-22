

namespace DataAccessLayer.Models
{
    public abstract class BasePump

    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsWireless { get; set; }
        public bool IsEnabled { get; set; }
        public byte Address { get; set; }
    }
}
