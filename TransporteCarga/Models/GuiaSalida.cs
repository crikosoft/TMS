using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class GuiaSalida
    {
        public int guiaSalidaId { get; set; }

        [Required(ErrorMessage = "Número de Serie de Guía requerido")]
        [DisplayName("Serie de Guía")]
        public string serie { get; set; }

        [Required(ErrorMessage = "Número de Guía requerido")]
        [DisplayName("Número de Guía")]
        public string numero { get; set; }
        public int ordenId { get; set; }
        public int? motivoTrasladoId { get; set; }

        [DisplayName("Detalle Motivo")]
        public string motivoDetalle { get; set; }

       ////Se utiliza fecha de Traslado de Envio
       // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
       // [DataType(DataType.Date)]
       // [DisplayName("Fecha Inicio de Traslado")]
       // public DateTime? fechaTraslado { get; set; }

        public virtual Orden Orden { get; set; }
        public virtual MotivoTraslado MotivoTraslado{ get; set; }
    }
}