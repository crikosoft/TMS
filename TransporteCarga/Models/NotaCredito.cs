using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class NotaCredito
    {
        public int notaCreditoId { get; set; }

        [DisplayName("Factura")]
        public int ventaId { get; set; }

        [DisplayName("Monto")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double monto { get; set; }

        [DisplayName("Concepto")]  
        public int notaCreditoConceptoId { get; set; }

        [DisplayName("Número Nota Crédito")]
        [StringLength(100)]
        public string numero { get; set; }


        [StringLength(100)]
        [DisplayName("Usuario Creación")]
        public string usuarioCreacion { get; set; }

        [StringLength(100)]
        [DisplayName("Usuario Modificación")]
        public string usuarioModificacion { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Fecha Creación")]
        public DateTime? fechaCreacion { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Fecha Modificación")]
        public DateTime? fechaModificacion { get; set; }

        public virtual Venta Venta { get; set; }
        public virtual NotaCreditoConcepto NotaCreditoConcepto { get; set; }
    }
}