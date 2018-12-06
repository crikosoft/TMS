using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class Direccion
    {
        public int direccionId { get; set; }

        [DisplayName("Dirección")]
        [Required(ErrorMessage = "Se requiere Dirección")]
        [StringLength(1000)]
        public string descripcion { get; set; }
        
        public int ubigeoId { get; set; }

        public int? orden { get; set; }

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

                
        public virtual Ubigeo Ubigeo { get; set; }

        public virtual List<Orden> OrigenesOrden { get; set; }
        public virtual List<Orden> DestinosOrden { get; set; }

        public virtual List<ClienteDireccion> ClienteDirecciones  { get; set; }

        [NotMapped]
        public string direccionCompleta
        {
            get
            {
                string retorna = "";

                if (this.Ubigeo != null)
                {
                    retorna = this.descripcion + " - " + this.Ubigeo.descripcion;

                    if (this.Ubigeo.UbigeoParent != null)
                    {
                        retorna = retorna + " - " + this.Ubigeo.UbigeoParent.descripcion;

                        if (this.Ubigeo.UbigeoParent.UbigeoParent != null)
                        {
                            retorna = retorna + " - " + this.Ubigeo.UbigeoParent.UbigeoParent.descripcion;
                        }
                    }
                }

                


                
                //string str = this.descripcion.Trim() + " " + this.Ubigeo.UbigeoParent.descripcion.Trim(); // +" " + this.Ubigeo.UbigeoParent.UbigeoParent.UbigeoParent.descripcion.Trim();
                //str.Split(' ').ToList().ForEach(i => retorna = retorna + i[0] + "");
                return retorna;

            }
            set
            {
            }
        }

    }
}