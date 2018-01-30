using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Ubigeo
    {
        public int ubigeoId { get; set; }

        [DisplayName("Código")]
        [Required(ErrorMessage = "Se requiere Código")]
        [StringLength(10)]
        public string codigo { get; set; }

        [DisplayName("Ubicación")]
        [Required(ErrorMessage = "Se requiere Ubicación")]
        [StringLength(1000)]
        public string descripcion { get; set; }

        // 1 distrito tiene multiples Código Postales
        //[DisplayName("Código Postal")]
        //[Required(ErrorMessage = "Se requiere Código Postal")]
        //[StringLength(100)]
        //public string codigoPostal { get; set; }
    }
}