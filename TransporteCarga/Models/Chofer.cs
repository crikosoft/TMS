using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Chofer
    {
        public int choferId { get; set; }
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "Se requiere Nombre")]
        [StringLength(200)]
        public string nombres { get; set; }

        [DisplayName("Apellidos")]
        [Required(ErrorMessage = "Se requiere Apellidos")]
        [StringLength(200)]
        public string apellidos { get; set; }

        [DisplayName("Documento Identidad")]
        [StringLength(20)]
        public string nroDocumentoIdentidad { get; set; }

        [DisplayName("Tipo Documento Identidad")]
        public int? tipoDocumentoIdentodadId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [DisplayName("Fecha Nacimiento")]
        public DateTime? fechaNacimiento { get; set; }

        [Display(Name = "Brevete")]
        [Required(ErrorMessage = "Se requiere Brevete")]
        [StringLength(50)]
        public string numeroBrevete { get; set; }

        public int proveedorId { get; set; }
        public virtual Proveedor Proveedor { get; set; }

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

        [NotMapped]
        public string iniciales
        {
            get
            {
                
                
               string retorna="";
               string str = this.nombres + " " + this.apellidos;
               str.Split(' ').ToList().ForEach(i => retorna = retorna + i[0] + "");
               return retorna;

            }
            set
            {
            }
        }
    }
}