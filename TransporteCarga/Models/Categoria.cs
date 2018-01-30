using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Categoria
    {
        [DisplayName("Categoria")]
        public int categoriaId { get; set; }

        [DisplayName("Nombre Categoria")]
        [Required(ErrorMessage = "Se requiere Nombre de Categoria")]
        [StringLength(1000)]
        public string nombre { get; set; }

        [DisplayName("Descripción Categoria")]
        [StringLength(1000)]
        public string descripcion { get; set; }

        public virtual List<Producto> Productos { get; set; }
    }
}