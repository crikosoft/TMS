using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    //Shipment_Order
    public class Orden
    {

        public Orden()
    {
        this.Envios = new List<Envio>();
        this.Ventas = new List<Venta>();
    }

        public int ordenId { get; set; }

        [DisplayName("Orden")]
        [StringLength(20)]
        public string numero { get; set; }


        public int clienteOrigenId { get; set; }
        public int clienteDestinatarioId { get; set; }
        public int clientePagoId { get; set; }

        public int? estadoOrdenId { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double subTotal { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double igv { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double Total { get; set; }

        public int? direccionOrigenId { get; set; }
        public int? direccionDestinoId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? fechaSolicitud { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? fechaEntrega { get; set; }

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

        [StringLength(1000)]
        public string comentario { get; set; }

        [StringLength(5000)]
        public string resumen { get; set; }


        public virtual EstadoOrden EstadoOrden{ get; set; }
        public virtual List<OrdenDetalle> Detalles { get; set; }
        public virtual List<Venta> Ventas { get; set; }
        public virtual List<GuiaSalida> GuiasSalida { get; set; }
        public virtual List<OrdenEstadoOrden> Estados { get; set; }
        public virtual List<Envio> Envios { get; set; }

        public virtual Direccion DireccionOrigen { get; set; }
        public virtual Direccion DireccionDestino { get; set; }
        public virtual Cliente ClienteDestinatario { get; set; }
        public virtual Cliente ClienteOrigen { get; set; }
        public virtual Cliente ClientePago { get; set; }



    }
}