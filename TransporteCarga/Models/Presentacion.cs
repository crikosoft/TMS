using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Presentacion
    {
        [DisplayName("Presentacion")]
        public int presentacionId { get; set; }

        [DisplayName("Nombre Presentación")]
        [Required(ErrorMessage = "Se requiere Nombre de Presentación")]
        [StringLength(1000)]
        public string nombre { get; set; }

        [DisplayName("Descripción Presentación")]
        [StringLength(1000)]
        public string descripcion { get; set; }



    }
}