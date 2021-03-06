﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class EstadoCompra
    {
        public int estadoCompraId { get; set; }
        [Required()]
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public virtual List<Compra> Compras { get; set; }
    }
}