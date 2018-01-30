using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class EstadoPago
    {
        public int estadoPagoId { get; set; }

        [Required()]
        [StringLength(100)]
        [DisplayName("Estado")]
        public string nombre { get; set; }

        [StringLength(500)]
        public string descripcion { get; set; }
        public virtual List<Pago> Pagos { get; set; }
    }
}