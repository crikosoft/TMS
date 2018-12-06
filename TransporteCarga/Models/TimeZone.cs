using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class TimeZone
    {
        public int timeZoneId { get; set; }
        public string name { get; set; }
        public int BaseUtcOffset { get; set; }
    }
}