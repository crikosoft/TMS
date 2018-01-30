using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class MotivoTraslado
    {
        public int motivoTrasladoId { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }
}