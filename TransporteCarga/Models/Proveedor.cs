using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{ //Service_Company
    public class Proveedor
    {
        [DisplayName("Proveedor")]
        public int proveedorId { get; set; }

        [DisplayName("Razón Social Proveedor")]
        [Required(ErrorMessage = "Se requiere Razón Social")]
        [StringLength(1000)]
        public string razonSocial { get; set; }

        [DisplayName("Contacto")]
        [StringLength(1000)]
        public string contacto { get; set; }

        [DisplayName("RUC")]
        [StringLength(50)]
        public string ruc { get; set; }

        [DisplayName("Dirección")]
        [StringLength(1000)]
        public string direccion { get; set; }

        [DisplayName("Teléfono")]
        [StringLength(50)]
        public string telefono { get; set; }

        [EmailAddress(ErrorMessage = "Formato de Email inválido")]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [DisplayName("Banco")]
        public int? bancoId { get; set; }

        [DisplayName("Nro de Cuenta")]
        [StringLength(200)]
        public string nroCuenta { get; set; }

        public virtual Banco Banco { get; set; }

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


    }
}