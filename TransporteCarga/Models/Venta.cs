using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Venta
    {
        public int ventaId { get; set; }
        public string serie { get; set; }
        public string numero { get; set; }
        public int ordenId { get; set; }
        public int tipoDocumentoId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha Emisión")]
        [DataType(DataType.Date)]
        public DateTime fecha { get; set; }
        public int estadoVentaId { get; set; }
        public int formaPagoId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha Pago Programado")]
        [DataType(DataType.Date)]
        public DateTime? fechaPagoProgramado { get; set; }


        [DisplayName("Fecha Pago Real")]
        [DataType(DataType.Date)]
        public DateTime? fechaPagoReal { get; set; }

        public virtual TipoDocumento TipoDocumento { get; set; }
        public virtual Orden Orden { get; set; }
        public virtual EstadoVenta EstadoVenta { get; set; }
        public virtual FormaPago FormaPago { get; set; }
    }
}