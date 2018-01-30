using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Distrito
    {
        [DisplayName("Distrito")]
        public int distritoId { get; set; }

        [DisplayName("Nombre Distrito")]
        [StringLength(1000)]
        public string nombre { get; set; }
    }
}