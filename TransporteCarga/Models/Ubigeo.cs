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

        public int? ubigeoParentId { get; set; }

        public virtual Ubigeo UbigeoParent { get; set; }


        //public virtual string departamento {
        //    get{

        //        return  this.codigo.Substring(0,2)+"0000";
        //    }
            
        //}


        //public virtual string provincia
        //{
        //    get
        //    {

        //        return this.codigo.Substring(0, 4) + "00";
        //    }

        //}
        // 1 distrito tiene multiples Código Postales
        //[DisplayName("Código Postal")]
        //[Required(ErrorMessage = "Se requiere Código Postal")]
        //[StringLength(100)]
        //public string codigoPostal { get; set; }
    }
}