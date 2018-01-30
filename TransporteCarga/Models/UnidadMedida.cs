using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class UnidadMedida
    {

        public int unidadMedidaId { get; set; }

        [DisplayName("Unidad Medida")]
        [Required(ErrorMessage = "Se requiere Unidad de Medida")]
        [StringLength(100)]
        public string nombre { get; set; }

        [DisplayName("Sigla Und. Med.")]
        [Required(ErrorMessage = "Se requiere Sigla de Unidad de Medida")]
        [StringLength(20)]
        public string sigla { get; set; }
        public List<Producto> Productos { get; set; }
    }
}