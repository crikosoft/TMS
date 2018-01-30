using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Precio
    {
        public int precioId { get; set; }
        public double precio { get; set; }
        public DateTime fecha { get; set; }
        public int productoId { get; set; }
        public int compraDetalleId { get; set; }
        public string usuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }

        public virtual Producto Producto { get; set; }
        public virtual CompraDetalle CompraDetalle { get; set; }

    }
}