using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class OrdenEstadoOrden
    {
        public int ordenEstadoOrdenId { get; set; }

        public int ordenId { get; set; }
        public virtual Orden Orden { get; set; }

        public int estadoOrdenId { get; set; }
        public virtual EstadoOrden EstadoOrden { get; set; }

        public string comentario { get; set; }

        public string usuarioCreacion { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime fechaCreacion { get; set; }
    }
}