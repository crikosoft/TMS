using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class TipoDocumento
    {
        public int tipoDocumentoId { get; set; }

        [DisplayName("Nombre")]
        [StringLength(50)]
        public string nombre { get; set; }

        [DisplayName("Descipción")]
        [StringLength(50)]
        public string descripcion { get; set; }
    }
}