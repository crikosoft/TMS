using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    //Shipment_Order_Details
    public class OrdenDetalle
    {
        public int ordenDetalleId { get; set; }
        public int productoId { get; set; }
        public double cantidad { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double precioUnitario { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double precioTotal { get; set; }
        public int ordenId { get; set; }
        public string comentario { get; set; }

        public virtual Orden Orden { get; set; }
        public virtual Producto Producto{ get; set; }

    }
}