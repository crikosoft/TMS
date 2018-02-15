using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Envio
    {


        public Envio()
        {
            this.Ordenes = new List<Orden>();
        }
        public int envioId { get; set; }

        public int proveedorId { get; set; }
        public int choferId { get; set; }
        public int vehiculoId { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public double? subTotal { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public double? igv { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public double? Total { get; set; }

        [DisplayName("Forma de Pago")]
        public int? formaPagoId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha Pago Programado")]
        [DataType(DataType.Date)]
        public DateTime? fechaPagoProgramado { get; set; }

        [Required(ErrorMessage = "Fecha Inicio de Traslado es requerido")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [DisplayName("Fecha Inicio de Traslado")]
        public DateTime fechaTraslado { get; set; }


        [StringLength(1000)]
        public string comentario { get; set; }

        [StringLength(100)]
        public string usuarioCreacion { get; set; }

        [StringLength(100)]
        public string usuarioModificacion { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime? fechaCreacion { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime? fechaModificacion { get; set; }

        public int estadoEnvioId { get; set; }

        public int[] OrdenIds { get; set; }

        public virtual List<Orden> Ordenes { get; set; }
        public virtual Proveedor Proveedor { get; set; }
        public virtual Chofer Chofer { get; set; }
        public virtual Vehiculo Vehiculo { get; set; }
        public virtual EstadoEnvio EstadoEnvio{ get; set; }
        public virtual FormaPago FormaPago { get; set; }
        public virtual List<Pago> Pagos { get; set; }
    }
}