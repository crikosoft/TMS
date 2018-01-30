using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Banco
    {
        public int bancoId { get; set; }
        [DisplayName("Nombre Banco")]
        [Required(ErrorMessage = "Se requiere Razón Social")]
        [StringLength(100)]
        public string nombre { get; set; }

        [DisplayName("Descripción Proveedor")]
        [StringLength(1000)]
        public string descripcion { get; set; }
    }
}