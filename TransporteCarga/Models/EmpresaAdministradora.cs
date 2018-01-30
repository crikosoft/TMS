using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class EmpresaAdministradora
    {
        [DisplayName("Empresa Administradora")]
        public int empresaAdministradoraId { get; set; }

        [DisplayName("Razón Social Administradora")]
        [Required(ErrorMessage = "Se requiere Razón Social")]
        [StringLength(1000)]
        public string RazonSocial { get; set; }

        [DisplayName("RUC")]
        [StringLength(50)]
        public string RUC { get; set; }

        [DisplayName("Dirección")]
        [StringLength(1000)]
        public string Direccion { get; set; }

    }
}