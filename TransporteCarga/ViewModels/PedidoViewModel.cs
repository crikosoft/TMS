using TransporteCarga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransporteCarga.ViewModels
{
    public class OrdenViewModel
    {

        public int? ordenViewModelId { get; set; }
        public List<Cliente> Clientes { get; set; }
        public List<Producto> Productos { get; set; }
        public Orden Orden { get; set; }

        public OrdenViewModel(List<Cliente> clientes, List<Producto> productos, Orden orden)
        {
            Clientes = clientes;
            Productos = productos;
            Orden = orden;
        }


    }
}