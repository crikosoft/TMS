using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TransporteCarga.Validator;

namespace TransporteCarga.Models
{
    public class Venta
    {
        public int ventaId { get; set; }
        public string serie { get; set; }

        [DisplayName("Nro Doc Venta")]
        public string numero { get; set; }
        public int ordenId { get; set; }
        public int tipoDocumentoId { get; set; }

        
        [DisplayName("Fecha Emisión")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime fecha { get; set; }
        public int estadoVentaId { get; set; }
        public int formaPagoId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha Pago Programado")]
        [DataType(DataType.DateTime)]
        public DateTime? fechaPagoProgramado { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha Pago Real")]
        [DataType(DataType.DateTime)]
        public DateTime? fechaPagoReal { get; set; }

        [DisplayName("Pago de S.P.O.T.")]
        public bool spot { get; set; }

        [CustomValidator]
        //[Required(ErrorMessage = "Guías son obligatorias")]
        public int[] OrdenIds { get; set; }

        public virtual TipoDocumento TipoDocumento { get; set; }
        public virtual List<Orden> Ordenes { get; set; }
        public virtual EstadoVenta EstadoVenta { get; set; }
        public virtual FormaPago FormaPago { get; set; }

        public virtual List<NotaCredito> NotasCredito { get; set; }
    }
}