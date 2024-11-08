using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public abstract class BaseSensor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsWireless { get; set; }
        public double LastReading { get; set; }
        public DateTime LastReadingTime { get; set; }

        public byte Address { get; set; }


        public void UpdateTemperature(double newReading)
        {
            LastReading = newReading;
            LastReadingTime = DateTime.Now;
        }
    }
}
