using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class ClienteDireccion
    {
        public int clienteDireccionId { get; set; }
        public int clienteId { get; set; }
        public int direccionId { get; set; }
        public int tipoDireccionId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? fechaInicio { get; set; }


        public virtual Cliente Cliente { get; set; }
        public virtual Direccion Direccion { get; set; }
        public virtual TipoDireccion TipoDireccion{ get; set; }
    }
}