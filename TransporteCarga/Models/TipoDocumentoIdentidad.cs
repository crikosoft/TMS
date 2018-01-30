using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class TipoDocumentoIdentidad
    {
        public int tipoDocumentoIdentidadId { get; set; }
        
        [DisplayName("Tipo Documento Identidad")]
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }
}