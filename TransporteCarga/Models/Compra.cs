using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Compra
    {
        public int compraId { get; set; }
        public int tipoDocumentoId { get; set; }
        public string serie { get; set; }
        public string numero { get; set; }
        public int proveedorId { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double subTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double igv { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double Total { get; set; }
        public string comentario { get; set; }
        public int estadoCompraId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? fechaDocumento { get; set; }

        public string usuarioCreacion { get; set; }
        public string usuarioModificacion { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime fechaCreacion { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime fechaModificacion { get; set; }

        public virtual Proveedor Proveedor { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }
        public virtual List<CompraDetalle> ComprasDetalles { get; set; }
        public virtual EstadoCompra EstadoCompra { get; set; }
    }
}