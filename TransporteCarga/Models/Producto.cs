using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Producto
    {
        [DisplayName("Producto")]
        public int productoId { get; set; }

        [DisplayName("Nombre Producto")]
        [Required(ErrorMessage = "Se requiere Nombre Producto")]
        [StringLength(1000)]
        public string nombre { get; set; }

        [DisplayName("Descripción Producto")]
        [StringLength(1000)]
        public string descripcion { get; set; }

        [DisplayName("Unidad Medida")]
        public int unidadMedidaId { get; set; }

        [DisplayName("Categoria Producto")]
        public int? categoriaId { get; set; }

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

        public virtual Categoria Categoria { get; set; }
        public virtual UnidadMedida UnidadMedida { get; set; }

        public virtual List<OrdenDetalle> Ordenes { get; set; }
        public virtual List<CompraDetalle> Compras { get; set; }
        public virtual List<Precio> Precios { get; set; }
    }
}