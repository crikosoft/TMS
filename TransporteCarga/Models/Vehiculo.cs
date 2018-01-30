using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Vehiculo
    {
        public int vehiculoId { get; set; }
        [DisplayName("Placa Unidad")]
        [Required(ErrorMessage = "Se requiere Placa Unidad de Vehículo")]
        [StringLength(50)]
        public string placaUnidad { get; set; }

        [DisplayName("Placa Carretera")]
        [StringLength(50)]
        public string placaCarretera { get; set; }

        [DisplayName("Configuración Vehicular")]
        [StringLength(50)]
        public string configuracionVehicular { get; set; }
        [DisplayName("Certificado de Inscripción 1")]
        [StringLength(50)]
        public string certificadoInscripcion1 { get; set; }

        [DisplayName("Certificado de Inscripción 2")]
        [StringLength(50)]
        public string certificadoInscripcion2 { get; set; }

        [DisplayName("Marca")]
        [StringLength(100)]
        public string Marca { get; set; }

        [DisplayName("Modelo")]
        [StringLength(100)]
        public string Modelo { get; set; }

        public int proveedorId { get; set; }

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

        public virtual Proveedor Proveedor { get; set; }
    }
}