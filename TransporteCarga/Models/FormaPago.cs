using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class FormaPago
    {
        public int formaPagoId { get; set; }

        [Required()]
         [StringLength(100)]
        public string nombre { get; set; }

         [StringLength(500)]
        public string descripcion { get; set; }

         public virtual List<Venta> Ventas { get; set; }
         public virtual List<Envio> Envios { get; set; }
    }
}