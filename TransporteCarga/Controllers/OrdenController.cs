using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TransporteCarga.Models;

namespace TransporteCarga.Controllers
{
    public class OrdenController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /Orden/
        public ActionResult Index()
        {
            ViewBag.clienteId = new SelectList(db.Clientes.OrderBy(a => a.razonSocial), "razonSocial", "razonSocial");
            var ordenes = db.Ordenes.Where(a => a.EstadoOrden.nombre != "Anulado").Include(p => p.ClienteOrigen).Include(p => p.EstadoOrden).OrderByDescending(a=>a.ordenId);

            //var ordenes = db.Ordenes.Include(o => o.Cliente).Include(o => o.DireccionDestino).Include(o => o.DireccionOrigen).Include(o => o.EstadoOrden);
            return View(ordenes.ToList());
        }


        public ActionResult IndexRentabilidad(DateTime? start, DateTime? end)
        {
            ViewBag.start = start;
            ViewBag.end = end;

            //var estadoList = new string[] { "Creado", "Entregado", "Con Factura", "Con Guia", "Pagado" };

            List<Orden> ordenes = new List<Orden>();
            if (start != null && end != null)
            {
                //                pedidos = db.Pedidos.Where(a => a.EstadoPedido.nombre!= "Anulado").Where(a => DbFunctions.TruncateTime(a.fechaCreacion) >= start && DbFunctions.TruncateTime(a.fechaCreacion) <= end).ToList();
                ordenes = db.Ordenes.Where(a => a.EstadoOrden.nombre != "Anulado").Where(a => DbFunctions.TruncateTime(a.Ventas.FirstOrDefault().fecha) >= start && DbFunctions.TruncateTime(a.Ventas.FirstOrDefault().fecha) <= end).ToList();

            }
            else
            {
                ordenes = db.Ordenes.Where(a => a.EstadoOrden.nombre != "Anulado").ToList();
            }

            return View(ordenes.ToList());
        }


        public ActionResult IndexGuias(DateTime? start, DateTime? end)
        {
            ViewBag.start = start;
            ViewBag.end = end;

            //var estadoList = new string[] { "Creado", "Entregado", "Con Factura", "Con Guia", "Pagado" };

            List<Orden> ordenes = new List<Orden>();
            if (start != null && end != null)
            {
                //                pedidos = db.Pedidos.Where(a => a.EstadoPedido.nombre!= "Anulado").Where(a => DbFunctions.TruncateTime(a.fechaCreacion) >= start && DbFunctions.TruncateTime(a.fechaCreacion) <= end).ToList();
                ordenes = db.Ordenes.Where(a => a.EstadoOrden.nombre != "Anulado").Where(a => DbFunctions.TruncateTime(a.fechaCreacion) >= start && DbFunctions.TruncateTime(a.fechaCreacion) <= end).ToList();

            }
            else
            {
                ordenes = db.Ordenes.Where(a => a.EstadoOrden.nombre != "Anulado").ToList();
            }

            return View(ordenes.ToList());
        }

        public ActionResult IndexFacturas(DateTime? start, DateTime? end)
        {
            ViewBag.start = start;
            ViewBag.end = end;

            //var estadoList = new string[] { "Creado", "Entregado", "Con Factura", "Con Guia", "Pagado" };

            List<Orden> ordenes = new List<Orden>();
            if (start != null && end != null)
            {
                //                pedidos = db.Pedidos.Where(a => a.EstadoPedido.nombre!= "Anulado").Where(a => DbFunctions.TruncateTime(a.fechaCreacion) >= start && DbFunctions.TruncateTime(a.fechaCreacion) <= end).ToList();
                ordenes = db.Ordenes.Where(a => a.EstadoOrden.nombre != "Anulado").Where(a => DbFunctions.TruncateTime(a.fechaCreacion) >= start && DbFunctions.TruncateTime(a.fechaCreacion) <= end).ToList();

            }
            else
            {
                ordenes = db.Ordenes.Where(a => a.EstadoOrden.nombre != "Anulado").ToList();
            }

            return View(ordenes.ToList());
        }

        // GET: /Orden/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Ordenes.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        // GET: /Orden/Create
        public ActionResult Create()
        {
            ViewBag.clienteId = new SelectList(db.Clientes.OrderBy(a => a.razonSocial), "clienteid", "razonSocial");
            ViewBag.direccionDestinoId = new SelectList(db.Direcciones, "direccionId", "descripcion");
            ViewBag.direccionOrigenId = new SelectList(db.Direcciones, "direccionId", "descripcion");
            ViewBag.estadoOrdenId = new SelectList(db.EstadosOrden, "estadoOrdenId", "nombre");
            return View();
        }

        // POST: /Orden/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ordenId,numero,clienteId,estadoOrdenId,subTotal,igv,Total,direccionOrigenId,direccionDestinoId,fechaSolicitud,fechaEntrega,usuarioCreacion,usuarioModificacion,fechaCreacion,fechaModificacion,comentario")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                db.Ordenes.Add(orden);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.clienteId = new SelectList(db.Clientes.OrderBy(a => a.razonSocial), "clienteid", "razonSocial", orden.clienteOrigenId);
            ViewBag.direccionDestinoId = new SelectList(db.Direcciones, "direccionId", "descripcion", orden.direccionDestinoId);
            ViewBag.direccionOrigenId = new SelectList(db.Direcciones, "direccionId", "descripcion", orden.direccionOrigenId);
            ViewBag.estadoOrdenId = new SelectList(db.EstadosOrden, "estadoOrdenId", "nombre", orden.estadoOrdenId);
            return View(orden);
        }

        // GET: /Orden/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Ordenes.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            ViewBag.clienteId = new SelectList(db.Clientes.OrderBy(a => a.razonSocial), "clienteid", "razonSocial", orden.clienteOrigenId);
            ViewBag.direccionDestinoId = new SelectList(db.Direcciones, "direccionId", "descripcion", orden.direccionDestinoId);
            ViewBag.direccionOrigenId = new SelectList(db.Direcciones, "direccionId", "descripcion", orden.direccionOrigenId);
            ViewBag.estadoOrdenId = new SelectList(db.EstadosOrden, "estadoOrdenId", "nombre", orden.estadoOrdenId);
            return View(orden);
        }

        // POST: /Orden/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ordenId,numero,clienteId,estadoOrdenId,subTotal,igv,Total,direccionOrigenId,direccionDestinoId,fechaSolicitud,fechaEntrega,usuarioCreacion,usuarioModificacion,fechaCreacion,fechaModificacion,comentario")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orden).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.clienteId = new SelectList(db.Clientes.OrderBy(a => a.razonSocial), "clienteid", "razonSocial", orden.clienteOrigenId);
            ViewBag.direccionDestinoId = new SelectList(db.Direcciones, "direccionId", "descripcion", orden.direccionDestinoId);
            ViewBag.direccionOrigenId = new SelectList(db.Direcciones, "direccionId", "descripcion", orden.direccionOrigenId);
            ViewBag.estadoOrdenId = new SelectList(db.EstadosOrden, "estadoOrdenId", "nombre", orden.estadoOrdenId);
            return View(orden);
        }

        // GET: /Orden/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Ordenes.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        // POST: /Orden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orden orden = db.Ordenes.Find(id);
            db.Ordenes.Remove(orden);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
