using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Cliente
    {
        [DisplayName("Cliente")]
        public int clienteid { get; set; }

        [DisplayName("Razón Social Cliente")]
        [Required(ErrorMessage = "Se requiere Razón Social")]
        [StringLength(1000)]
        public string razonSocial { get; set; }

        [DisplayName("Contacto")]
        [StringLength(1000)]
        public string contacto { get; set; }

        [DisplayName("RUC o DNI")]
        [Required(ErrorMessage = "Se requiere RUC o DNI")]
        [StringLength(50)]
        public string ruc { get; set; }

        [DisplayName("Teléfono")]
        [StringLength(50)]
        public string telefono { get; set; }

        [EmailAddress(ErrorMessage = "Formato de Email inválido")]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(200)]
        public string email { get; set; }

        public virtual List<Orden> Ordenes { get; set; }

        public virtual List<ClienteDireccion> Direcciones { get; set; }

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

        public virtual List<Orden> OrdenesDestinos { get; set; }
        public virtual List<Orden> OrdenesOrigenes { get; set; }
        public virtual List<Orden> OrdenesPagos { get; set; }



    }
}