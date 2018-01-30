using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TransporteCarga.ViewModels;
using TransporteCarga.Models;

namespace KleanKart.Controllers
{
    public class OrdenViewModelController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();


        // GET: /RequerimientoViewModel/Create/1
        public ActionResult Create(int? id)
        {
            var clientes = db.Clientes.OrderBy(a => a.razonSocial);
            var productos = db.Productos;
            Orden pedido = new Orden();           
            var OrdenViewModels = new OrdenViewModel(clientes.ToList(), productos.ToList(), pedido);



            var queryDirecciones = from q in db.Direcciones
                 select new
                 {
                   direccionId = q.direccionId,
                   descripcion = q.descripcion + " - " + q.Ubigeo.descripcion
                 };

            ViewBag.direccionOrigenId = new SelectList(queryDirecciones, "direccionId", "descripcion");
            ViewBag.direccionDestinoId = new SelectList(queryDirecciones, "direccionId", "descripcion");


            return View(OrdenViewModels);
        }


        // GET: /RequerimientoViewModel/Create/1
        public ActionResult Edit(int? id)
        {
            var clientes = db.Clientes.OrderBy(a => a.razonSocial);
            var productos = db.Productos;
            var pedido = db.Ordenes.Find(id) ;
            var OrdenViewModels = new OrdenViewModel(clientes.ToList(), productos.ToList(), pedido);
            ViewBag.Cliente = new SelectList(db.Clientes.OrderBy(a =>a.razonSocial), "clienteId", "razonSocial", pedido.clienteOrigenId);
            ViewBag.Remitente = new SelectList(db.Clientes.OrderBy(a => a.razonSocial), "clienteId", "razonSocial", pedido.ClienteOrigen.clienteid);
            ViewBag.Destinatario = new SelectList(db.Clientes.OrderBy(a => a.razonSocial), "clienteId", "razonSocial", pedido.ClienteDestinatario.clienteid);
            var queryDirecciones = from q in db.Direcciones
                                   select new
                                   {
                                       direccionId = q.direccionId,
                                       descripcion = q.descripcion + " - " + q.Ubigeo.descripcion
                                   };

            ViewBag.direccionOrigenId = new SelectList(queryDirecciones, "direccionId", "descripcion", pedido.direccionOrigenId);
            ViewBag.direccionDestinoId = new SelectList(queryDirecciones, "direccionId", "descripcion", pedido.direccionDestinoId);
            return View(OrdenViewModels);
        }

        [HttpPost]
        public ActionResult CrearOrden(Orden orden)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                
                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

                orden.fechaCreacion = cstTime;
                orden.usuarioCreacion = User.Identity.Name;
                orden.fechaModificacion = cstTime;
                orden.usuarioModificacion = User.Identity.Name;

                var estadoOrden = db.EstadosOrden.Single(p => p.nombre == "Creado");

                orden.estadoOrdenId = estadoOrden.estadoOrdenId;

                db.Ordenes.Add(orden);


                // Grabar Estado de Orden
                var ordenEstadoOrden = new OrdenEstadoOrden();

                ordenEstadoOrden.ordenId = orden.ordenId;
                ordenEstadoOrden.estadoOrdenId = estadoOrden.estadoOrdenId;
                ordenEstadoOrden.usuarioCreacion = User.Identity.Name;
                ordenEstadoOrden.fechaCreacion = cstTime;

                db.OrdenesEstadoOrden.Add(ordenEstadoOrden);

                db.SaveChanges();

                return RedirectToAction("Index", "Orden");
            }

            return View(orden);
        }

        [HttpPost]
        public ActionResult EditarOrden(Orden orden)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {

                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
                Orden ordenOriginal = db.Ordenes.Find(orden.ordenId);

                ordenOriginal.fechaModificacion = cstTime;
                ordenOriginal.usuarioModificacion = User.Identity.Name;
                ordenOriginal.clienteOrigenId = orden.clienteOrigenId;
                ordenOriginal.clienteDestinatarioId = orden.clienteDestinatarioId;
                ordenOriginal.direccionOrigenId = orden.direccionOrigenId;
                ordenOriginal.direccionDestinoId = orden.direccionDestinoId;
                ordenOriginal.clientePagoId = orden.clientePagoId;
                ordenOriginal.comentario = orden.comentario;
                ordenOriginal.subTotal = orden.subTotal;
                ordenOriginal.igv = orden.igv;
                ordenOriginal.Total = orden.Total;
                    
                foreach (var item in ordenOriginal.Detalles.ToList())
                {
                    var ped = item;
                    db.OrdenDetalles.Remove(ped);

                    //Producto producto = db.Productos.Find(item.productoId);
                    //if (producto != null)
                    //{
                    //    producto.stock = producto.stock + item.cantidad;
                    //}

                }


                foreach (var item in orden.Detalles)
                {
                    db.OrdenDetalles.Add(item);
                }


                ////Actualiza Stock
                //foreach (var item in orden.Detalles)
                //{
                //    Producto producto = db.Productos.Find(item.productoId);
                //    if (producto != null)
                //    {
                //        producto.stock = producto.stock - item.cantidad;
                //    }
                //}

                db.SaveChanges();

                return RedirectToAction("Index", "Orden");
            }

            return View(orden);
        }




    }
}
