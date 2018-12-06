using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class NotaCreditoConcepto
    {
        public int notaCreditoConceptoId { get; set; }

        [DisplayName("Concepto")]
        public string concepto { get; set; }
    }
}