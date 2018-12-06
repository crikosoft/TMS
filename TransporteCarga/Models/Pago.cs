using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Pago
    {

        public int pagoId { get; set; }
        public int envioId { get; set; }
        public int tipoDocumentoId { get; set; }
        [DisplayName("Número Documento")]
        public string numero { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha de Pago Programado")]
        [DataType(DataType.Date)]
        public DateTime? fechaPagoProg { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha de Pago Real")]
        [DataType(DataType.Date)]
        public DateTime? fechaPagoReal { get; set; }

        public int estadoPagoId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [DisplayName("Monto Pago")]
        public double? pagoMonto { get; set; }

        [DisplayName("Monto Detracción")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double? pagoDetraccion { get; set; }
        public string asientoContable { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha Contable")]
        [DataType(DataType.Date)]
        public DateTime? fechaContable { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha Emisión Factura")]
        [DataType(DataType.Date)]
        public DateTime? fechaFactura { get; set; }

        [DisplayName("Usuario Creación")]
        public string usuarioCreacion { get; set; }
        public string usuarioModificacion { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha Creación")]
        public DateTime fechaCreacion { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? fechaModificacion { get; set; }

        public virtual Envio Envio { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }
        public virtual EstadoPago EstadoPago { get; set; }
       
        public bool preguntaPagoTotal { get; set; }
       

    }
}