using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Direccion
    {
        public int direccionId { get; set; }

        [DisplayName("Dirección")]
        [Required(ErrorMessage = "Se requiere Dirección")]
        [StringLength(1000)]
        public string descripcion { get; set; }
        
        public int ubigeoId { get; set; }

        public int? orden { get; set; }

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

                
        public virtual Ubigeo Ubigeo { get; set; }

        public virtual List<Orden> OrigenesOrden { get; set; }
        public virtual List<Orden> DestinosOrden { get; set; }

        public virtual List<ClienteDireccion> ClienteDirecciones  { get; set; }
    }
}