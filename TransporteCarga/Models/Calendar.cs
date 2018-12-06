using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Calendar
    {
        public string title { get; set; }
        public string start { get; set; }
        //public DateTime? end { get; set; }
        public bool allDay { get; set; }
        public string url { get; set; }
    }
}