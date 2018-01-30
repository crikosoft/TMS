using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class CompraDetalle
    {
        public int compraDetalleId { get; set; }
        public int productoId { get; set; }
        public double cantidad { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double precioUnitario { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double precioTotal { get; set; }
        public int compraId { get; set; }

        public virtual Compra Compra { get; set; }
        public virtual Producto Producto { get; set; }
    }
}