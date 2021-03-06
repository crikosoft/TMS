﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TransporteCarga.Models;
using Microsoft.Reporting.WebForms;
using TransporteCarga.Helpers;
using System.Globalization;

namespace KleanKart.Controllers
{
    public class VentaController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /Venta/
        public ActionResult Index()
        {

            ViewBag.clienteId = new SelectList(db.Clientes.OrderBy(a => a.razonSocial), "razonSocial", "razonSocial");
            //Identificador 1fact >1Guia
             var ventas = db.Ventas.Include(v => v.EstadoVenta).Include(v => v.Ordenes).Include(v => v.TipoDocumento);
            return View(ventas.ToList());
        }

        // GET: /Venta/
        public ActionResult IndexDetails()
        {

            ViewBag.clienteId = new SelectList(db.Clientes.OrderBy(a => a.razonSocial), "razonSocial", "razonSocial");
            ViewBag.productoId = new SelectList(db.Productos.OrderBy(a => a.nombre), "nombre", "nombre");
            ViewBag.estadoVentaId = new SelectList(db.EstadoVentas, "nombre", "nombre");
            //Identificador 1fact >1Guia
            var ventas = db.Ventas.Include(v => v.EstadoVenta).Include(v => v.Ordenes).Include(v => v.TipoDocumento);
            return View(ventas.ToList());
        }

        // GET: /Venta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // GET: /Venta/Create
        //public ActionResult Create(int? id)
        //{

        //    Venta Venta = new Venta();
        //    Orden Pedido = db.Ordenes.Where(a => a.ordenId == id).FirstOrDefault();
        //    Venta.ordenId = Pedido.ordenId;
        //    //Identificador 1fact >1Guia
        //    //Venta.Orden = Pedido;
        //    //Venta.Ordenes.Add( Pedido);

        //    var tipoDocumentosList = new string[] { "Factura de Venta", "Boleta de Venta" };



        //    ViewBag.formaPagoId = new SelectList(db.FormaPagos, "formaPagoId", "nombre");
        //    ViewBag.pedidoId = new SelectList(db.Ordenes, "ordenId", "numero");
        //    //ViewBag.tipoDocumentoId = new SelectList(db.TipoDocumentos, "tipoDocumentoId", "nombre");
        //    ViewBag.tipoDocumentoId = new SelectList(db.TipoDocumentos.Where(p => tipoDocumentosList.Contains(p.nombre)), "tipoDocumentoId", "nombre");
        //    ViewBag.pedidoId = new SelectList(db.Ordenes.Where(a => a.ordenId == id), "ordenId", "ordenId");

        //    return View(Venta);
        //}

        public ActionResult Create()
        {

            Venta Venta = new Venta();
            var tipoDocumentosList = new string[] { "Factura de Venta", "Boleta de Venta" };

            ViewBag.formaPagoId = new SelectList(db.FormaPagos, "formaPagoId", "nombre");
            ViewBag.pedidoId = new SelectList(db.Ordenes, "ordenId", "numero");
            ViewBag.tipoDocumentoId = new SelectList(db.TipoDocumentos.Where(p => tipoDocumentosList.Contains(p.nombre)), "tipoDocumentoId", "nombre");

            ViewBag.Ordenes = db.Ordenes.Where(a => a.Ventas.Count() == 0 && a.GuiasSalida.Count()>0 &&  a.Envios.Count()>0).Include(e => e.ClientePago).Include(e => e.DireccionOrigen).Include(e => e.DireccionDestino).Include(e => e.GuiasSalida);

            Envio model = new Envio
            {
                OrdenIds = new int[0]
            };

            return View(Venta);
        }



        // POST: /Venta/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ventaId,serie,numero,tipoDocumentoId,fecha, formaPagoId, spot,OrdenIds")] Venta venta)
        {
            if (ModelState.IsValid)
            {


                if (venta.OrdenIds != null)
                {
                    venta.Ordenes = db.Ordenes.Where(a => venta.OrdenIds.Contains(a.ordenId)).ToList();
                }





                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

                //Estado Venta
                var estadoVenta = db.EstadoVentas.Single(p => p.nombre == "Creado");
                venta.estadoVentaId = estadoVenta.estadoVentaId;

                var formaPago = db.FormaPagos.Find(venta.formaPagoId);

                switch (formaPago.nombre)
                {
                    case "Factura a 7 días":
                        venta.fechaPagoProgramado = cstTime.AddDays(7);
                        break;
                    case "Factura a 15 días":
                        venta.fechaPagoProgramado = cstTime.AddDays(15);
                        break;
                    case "Factura a 30 días":
                        venta.fechaPagoProgramado = cstTime.AddDays(30);
                        break;
                    case "Factura a 90 días":
                        venta.fechaPagoProgramado = cstTime.AddDays(90);
                        break;
                    default:
                        venta.fechaPagoProgramado = cstTime;
                        break;
                }



                //Actualiza estados de Ordenes
                var estadoFactura = db.EstadosOrden.Single(p => p.nombre == "Con Factura");

                foreach (var item in venta.Ordenes)
                {
                    item.estadoOrdenId = estadoFactura.estadoOrdenId;
                    
                }

                //var orden = db.Ordenes.Find(venta.ordenId);
                //orden.estadoOrdenId = estadoFactura.estadoOrdenId;
                //Identificador 1fact >1Guia
                //venta.Orden = orden;
                //venta.Ordenes.Add(orden);


                //2. Grabar Estado de Pedido
                var ordenEstadoOrden = new OrdenEstadoOrden();

                foreach (var item in venta.Ordenes)
                {
                    ordenEstadoOrden.ordenId = item.ordenId;
                    ordenEstadoOrden.estadoOrdenId = estadoFactura.estadoOrdenId;
                    ordenEstadoOrden.usuarioCreacion = User.Identity.Name;
                    ordenEstadoOrden.fechaCreacion = cstTime;
                    db.OrdenesEstadoOrden.Add(ordenEstadoOrden);

                }


                db.SaveChanges();



                db.Ventas.Add(venta);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "Venta");
            }

            ViewBag.estadoVentaId = new SelectList(db.EstadoVentas, "estadoVentaId", "nombre", venta.estadoVentaId);
            ViewBag.ordenId = new SelectList(db.Ordenes, "ordenId", "numero", venta.ordenId);
            ViewBag.tipoDocumentoId = new SelectList(db.TipoDocumentos, "tipoDocumentoId", "nombre", venta.tipoDocumentoId);
            ViewBag.formaPagoId = new SelectList(db.FormaPagos, "formaPagoId", "nombre", venta.formaPagoId);
            ViewBag.Ordenes = db.Ordenes.Where(a => a.Ventas.Count() == 0 && a.GuiasSalida.Count() > 0 && a.Envios.Count() > 0).Include(e => e.ClientePago).Include(e => e.DireccionOrigen).Include(e => e.DireccionDestino).Include(e => e.GuiasSalida);

            Envio model = new Envio
            {
                OrdenIds = new int[0]
            };

            return View(venta);
            //return RedirectToAction("Index", "Pedido");
        }

        // GET: /Venta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Ventas.Where(a => a.ventaId == id).Include(a => a.Ordenes).SingleOrDefault();
            if (venta == null)
            {
                return HttpNotFound();
            }
            ViewBag.formaPagoId = new SelectList(db.FormaPagos, "formaPagoId", "nombre", venta.formaPagoId);
            //ViewBag.ordenId = new SelectList(db.Ordenes, "ordenId", "numero", venta.ordenId);
            //ViewBag.Ordenes = db.Ordenes.Where(a => a.Ventas.Count() == 0 || a.Ventas.Any(b => b.ventaId == id)).Include(e => e.ClientePago).Include(e => e.DireccionOrigen).Include(e => e.DireccionDestino).Include(e => e.GuiasSalida);
            ViewBag.Ordenes = db.Ordenes.Where(a => (a.Ventas.Count() == 0 && a.GuiasSalida.Count() > 0 && a.Envios.Count() > 0) || a.Ventas.Any(b => b.ventaId == id)).Include(e => e.ClientePago).Include(e => e.DireccionOrigen).Include(e => e.DireccionDestino).Include(e => e.GuiasSalida);

            
            ViewBag.tipoDocumentoId = new SelectList(db.TipoDocumentos, "tipoDocumentoId", "nombre", venta.tipoDocumentoId);

            Venta model = new Venta
            {
                OrdenIds = new int[0]
            };
            
            return View(venta);
        }


       



        // POST: /Venta/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ventaId,serie,numero,tipoDocumentoId,fecha, formaPagoId, spot, OrdenIds")] Venta venta)
        {
            if (ModelState.IsValid)
            {
               
                var formaPago = db.FormaPagos.Find(venta.formaPagoId);

                switch (formaPago.nombre)
                {
                    case "Factura a 7 días":
                        venta.fechaPagoProgramado = venta.fecha.AddDays(7);
                        break;
                    case "Factura a 15 días":
                        venta.fechaPagoProgramado = venta.fecha.AddDays(15);
                        break;
                    case "Factura a 30 días":
                        venta.fechaPagoProgramado = venta.fecha.AddDays(30);
                        break;
                    case "Factura a 90 días":
                        venta.fechaPagoProgramado = venta.fecha.AddDays(90);
                        break;
                    default:
                        venta.fechaPagoProgramado = venta.fecha;
                        break;
                }

                var ventaOriginal = db.Ventas.Find(venta.ventaId);
                ventaOriginal.Ordenes.RemoveAll(a => a.Ventas.SingleOrDefault().ventaId == ventaOriginal.ventaId);

                if (venta.OrdenIds != null)
                {
                    var ordenes = db.Ordenes.Where(a => venta.OrdenIds.Contains(a.ordenId)).ToList();
                    foreach (var item in ordenes)
                    {
                        ventaOriginal.Ordenes.Add(item);
                    }
                }

                ventaOriginal.tipoDocumentoId = venta.tipoDocumentoId;
                ventaOriginal.serie = venta.serie;
                ventaOriginal.numero = venta.numero;
                ventaOriginal.fecha = venta.fecha;
                ventaOriginal.formaPagoId = venta.formaPagoId;
                ventaOriginal.fechaPagoProgramado = venta.fechaPagoProgramado;
                ventaOriginal.spot = venta.spot;
                ventaOriginal.OrdenIds = venta.OrdenIds;

                //db.Entry(ventaOriginal).State = EntityState.Modified;
                //db.Entry(ventaOriginal).Property(x => x.ordenId).IsModified = false;
                //db.Entry(ventaOriginal).Property(x => x.estadoVentaId).IsModified = false;



                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    foreach (var failure in ex.EntityValidationErrors)
                    {
                        sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                        foreach (var error in failure.ValidationErrors)
                        {
                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                            sb.AppendLine();
                        }
                    }

                    throw new System.Data.Entity.Validation.DbEntityValidationException(
                        "Entity Validation Failed - errors follow:\n" +
                        sb.ToString(), ex
                    ); // Add the original exception as the innerException
                }


                
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "Venta");
            }
            ViewBag.formaPagoId = new SelectList(db.FormaPagos, "formaPagoId", "nombre", venta.formaPagoId);
            //ViewBag.pedidoId = new SelectList(db.Ordenes, "pedidoId", "numero", venta.ordenId);
            ViewBag.tipoDocumentoId = new SelectList(db.TipoDocumentos, "tipoDocumentoId", "nombre", venta.tipoDocumentoId);
            ViewBag.Ordenes = db.Ordenes.Where(a => (a.Ventas.Count() == 0 && a.GuiasSalida.Count() > 0 && a.Envios.Count() > 0) || a.Ventas.Any(b => b.ventaId == venta.ventaId)).Include(e => e.ClientePago).Include(e => e.DireccionOrigen).Include(e => e.DireccionDestino).Include(e => e.GuiasSalida);

            Envio model = new Envio
            {
                OrdenIds = new int[0]
            };

            venta = db.Ventas.Where(a => a.ventaId == venta.ventaId).Include(a => a.Ordenes).SingleOrDefault();




            return View(venta);
        }


        // GET: /Venta/Edit/5
        public ActionResult EditPayment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            ViewBag.ordenId = new SelectList(db.Ordenes, "ordenId", "numero", venta.ordenId);
            return View(venta);
        }

        // POST: /Venta/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPayment([Bind(Include = "ventaId, fechaPagoReal, ordenIds")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(venta).State = EntityState.Modified;
                db.Entry(venta).Property(x => x.ordenId).IsModified = false;
                db.Entry(venta).Property(x => x.estadoVentaId).IsModified = false;
                 db.Entry(venta).Property(x => x.serie).IsModified = false;
                 db.Entry(venta).Property(x => x.numero).IsModified = false;
                 db.Entry(venta).Property(x => x.tipoDocumentoId).IsModified = false;
                 db.Entry(venta).Property(x => x.fecha).IsModified = false;
                 db.Entry(venta).Property(x => x.formaPagoId).IsModified = false;
                 db.Entry(venta).Property(x => x.fechaPagoProgramado).IsModified = false;

                if (venta.fechaPagoReal != null)
                {
                    DateTime timeUtc = DateTime.UtcNow;
                    TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                    DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

                    ////1. Actualiza estados de Pedido
                    //var estadoOrden = db.EstadosOrden.Single(p => p.nombre == "Cobrado");
                    //var ordenOriginal = db.Ordenes.Find(venta.ordenId);
                    //ordenOriginal.estadoOrdenId = estadoOrden.estadoOrdenId;
                    //venta.Ordenes.Add(ordenOriginal);

                    //Get VentaOriginal
                    var ventaOriginal = db.Ventas.Where(a=>a.ventaId == venta.ventaId).Include(a=>a.Ordenes);
                    //1. Actualiza estados de Pedido
                    var estadoOrden = db.EstadosOrden.Single(p => p.nombre == "Cobrado");
                    foreach (var orden in ventaOriginal.FirstOrDefault().Ordenes)
                    {
                        //var ordenOriginal = db.Ordenes.Find(orden.ordenId);
                        orden.estadoOrdenId = estadoOrden.estadoOrdenId;
                    }

                    //2. Grabar Estado de Pedido
                    var ordenEstadoOrden = new OrdenEstadoOrden();
                    foreach (var orden in ventaOriginal.FirstOrDefault().Ordenes)
                    {
                        ordenEstadoOrden.ordenId = orden.ordenId;
                        ordenEstadoOrden.estadoOrdenId = estadoOrden.estadoOrdenId;
                        ordenEstadoOrden.usuarioCreacion = User.Identity.Name;
                        ordenEstadoOrden.fechaCreacion = cstTime;

                        db.OrdenesEstadoOrden.Add(ordenEstadoOrden);
                    }

                    db.SaveChanges();
                }
                 
                return RedirectToAction("Index", "Venta");
            }

            venta = db.Ventas.Find(venta.ventaId);
            return View(venta);
        }



        // GET: /Venta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // POST: /Venta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Venta venta = db.Ventas.Find(id);
            Orden pedido = db.Ordenes.Find(venta.ordenId);
            db.Ventas.Remove(venta);
            if (venta.Ordenes != null)
            {            
                foreach (var item in venta.Ordenes)
                {
                    if (item.Estados.Where(a => a.EstadoOrden.nombre == "Con Guia").Count() > 0)
                    {
                        item.estadoOrdenId = db.EstadosOrden.Single(p => p.nombre == "Con Guia").estadoOrdenId;
                    }
                    else
                    {
                        item.estadoOrdenId = db.EstadosOrden.Single(p => p.nombre == "Creado").estadoOrdenId;
                    }

                    OrdenEstadoOrden pedidoEstado = new OrdenEstadoOrden();
                    pedidoEstado = item.Estados.Where(p => p.EstadoOrden.nombre == "Con Factura").FirstOrDefault();
                    db.OrdenesEstadoOrden.Remove(pedidoEstado);
                }
            }


          


            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Index", "Orden");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // GET: /Pedido/Document/5
        public ActionResult Document(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // GET: /Pedido/Document/5
        public ActionResult DocumentPrint(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // GET: /Venta/
        public FileResult DocumentFacturaPrint(int? id)
        {

            if (id == null)
            {
                return null;
            }

            //List<Venta> venta = db.Ventas.Where(a => a.ventaId == id).ToList();
            List<Venta> venta = db.Ventas.Where(a => a.ordenId == id).ToList();

            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Cliente");
            dt.Columns.Add("Direccion");
            dt.Columns.Add("RUC");
            dt.Columns.Add("SerieF");
            dt.Columns.Add("NumeroF");
            dt.Columns.Add("SubTotal");
            dt.Columns.Add("IGV");
            dt.Columns.Add("Total");
            dt.Columns.Add("SerieG");
            dt.Columns.Add("NumeroG");
            dt.Columns.Add("FechaF");
            dt.Columns.Add("FechaG");
            dt.Columns.Add("PuntoDestinoG");
            dt.Columns.Add("PuntoPartidaG");
            dt.Columns.Add("Cantidad");
            dt.Columns.Add("DescripcionP");
            dt.Columns.Add("PrecioUnitario");
            dt.Columns.Add("PrecioTotal");
            dt.Columns.Add("TotalTexto");

            foreach (var item in venta[0].Ordenes.FirstOrDefault().Detalles.OrderBy(a => a.Producto.nombre))
            { 
                DataRow _ravi = dt.NewRow();
                _ravi["Cliente"] = venta[0].Ordenes.FirstOrDefault().ClienteOrigen.razonSocial;
                _ravi["Direccion"] = venta[0].Ordenes.FirstOrDefault().ClienteOrigen.Direcciones[0].Direccion.descripcion;
                _ravi["RUC"] = venta[0].Ordenes.FirstOrDefault().ClienteOrigen.ruc;
                _ravi["SerieF"] = venta[0].serie;
                _ravi["NumeroF"] = venta[0].numero;
                _ravi["SubTotal"] = string.Format("{0:0,0.00}", venta[0].Ordenes.FirstOrDefault().subTotal);
                _ravi["IGV"] = string.Format("{0:0,0.00}", venta[0].Ordenes.FirstOrDefault().igv);

                _ravi["Total"] = string.Format("{0:0,0.00}", venta[0].Ordenes.FirstOrDefault().Total);
                _ravi["FechaF"] = "Lima, " + venta[0].fecha.Day + " de " + venta[0].fecha.ToString("MMMM").ToUpper() +"  del " + venta[0].fecha.Year;

                if (venta[0].Ordenes.FirstOrDefault().GuiasSalida.Count >= 1)
                {
                    string fechaTraslado = "";
                    //fechaTraslado = "Lima, " + venta[0].Orden.GuiasSalida.SingleOrDefault().fechaTraslado.Day + " de Agosto del" + venta[0].Orden.GuiasSalida.SingleOrDefault().fechaTraslado.Year;

                    _ravi["SerieG"] = venta[0].Ordenes.FirstOrDefault().GuiasSalida.SingleOrDefault().serie;
                    _ravi["NumeroG"] = venta[0].Ordenes.FirstOrDefault().GuiasSalida.SingleOrDefault().numero;
                    //_ravi["FechaG"] = venta[0].Orden.GuiasSalida.SingleOrDefault().fechaTraslado;
                }
                else
                {
                    _ravi["SerieG"] = "";
                    _ravi["NumeroG"] = "";
                    _ravi["FechaG"] = ""; 
                }
                _ravi["PuntoDestinoG"] = venta[0].Ordenes.FirstOrDefault().ClienteOrigen.Direcciones[0].Direccion.descripcion;
                _ravi["PuntoPartidaG"] = "CALLE AURELIO FERNANDEZ CONCHA 298-MIRAFLORES";
                _ravi["Cantidad"] = item.cantidad;
                _ravi["DescripcionP"] = item.Producto.nombre;
                _ravi["PrecioUnitario"] = string.Format("{0:0,0.00}", item.precioUnitario);
                _ravi["PrecioTotal"] = string.Format("{0:0,0.00}", item.precioTotal);
                _ravi["TotalTexto"] = "CIENTO OCHENTA Y OCHO y 10/100";

                dt.Rows.Add(_ravi);
            }

           
            Microsoft.Reporting.WebForms.LocalReport localreport = new LocalReport();
            ReportDataSource reportdatasource1 = new ReportDataSource("dsMaestroDetalle");
            reportdatasource1.Name = "dsMaestroDetalle";
            localreport.ReportPath = Server.MapPath("~/Reports/Factura.rdlc");
            reportdatasource1.Value = dt;
            localreport.DataSources.Add(reportdatasource1);

            string reportType = "pdf";
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            string mimeType;
            renderedBytes = localreport.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileResult DocumentFacturaEmptyPrint(int? id)
        {

            if (id == null)
            {
                return null;
            }
            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

            //1. Cambia Estado
            var pedidoOriginal = db.Ordenes.Find(id);
            var estadoPedido = db.EstadosOrden.Single(p => p.nombre == "Con Factura");

            pedidoOriginal.estadoOrdenId = estadoPedido.estadoOrdenId;
            pedidoOriginal.usuarioModificacion = User.Identity.Name;
            pedidoOriginal.fechaModificacion = cstTime;

            //2. Grabar Estado de Pedido
            var pedidoEstadoPedido = new OrdenEstadoOrden();

            pedidoEstadoPedido.ordenId = pedidoOriginal.ordenId;
            pedidoEstadoPedido.estadoOrdenId = estadoPedido.estadoOrdenId;
            pedidoEstadoPedido.usuarioCreacion = User.Identity.Name;
            pedidoEstadoPedido.fechaCreacion = cstTime;

            db.OrdenesEstadoOrden.Add(pedidoEstadoPedido);

            db.SaveChanges();



            List<Venta> venta = db.Ventas.Where(a => a.ordenId == id).ToList();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Cliente");
            dt.Columns.Add("Direccion");
            dt.Columns.Add("RUC");
            dt.Columns.Add("SerieF");
            dt.Columns.Add("NumeroF");
            dt.Columns.Add("SubTotal");
            dt.Columns.Add("IGV");
            dt.Columns.Add("Total");
            dt.Columns.Add("SerieG");
            dt.Columns.Add("NumeroG");
            dt.Columns.Add("FechaF");
            dt.Columns.Add("FechaG");
            dt.Columns.Add("PuntoDestinoG");
            dt.Columns.Add("PuntoPartidaG");
            dt.Columns.Add("Cantidad");
            dt.Columns.Add("DescripcionP");
            dt.Columns.Add("PrecioUnitario");
            dt.Columns.Add("PrecioTotal");
            dt.Columns.Add("TotalTexto");
            //Identificador 1fact >1Guia
            //foreach (var item in venta[0].Orden.Detalles.OrderBy(a => a.Producto.nombre))
            foreach (var item in venta[0].Ordenes.FirstOrDefault().Detalles.OrderBy(a => a.Producto.nombre))
            {
                DataRow _ravi = dt.NewRow();
                _ravi["Cliente"] = venta[0].Ordenes.FirstOrDefault().ClienteOrigen.razonSocial;
                _ravi["Direccion"] = venta[0].Ordenes.FirstOrDefault().ClienteOrigen.Direcciones[0].Direccion.descripcion;
                _ravi["RUC"] = venta[0].Ordenes.FirstOrDefault().ClienteOrigen.ruc;
                _ravi["SerieF"] = venta[0].serie;
                _ravi["NumeroF"] = venta[0].numero;
                _ravi["SubTotal"] = string.Format("{0:0,0.00}", venta[0].Ordenes.FirstOrDefault().subTotal);
                _ravi["IGV"] = string.Format("{0:0,0.00}", venta[0].Ordenes.FirstOrDefault().igv);
                //Identificador 1fact >1Guia
                _ravi["Total"] = string.Format("{0:0,0.00}", venta[0].Ordenes.FirstOrDefault().Total);

                string fullMonthName = venta[0].fecha.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper();
                _ravi["FechaF"] = "Lima, " + venta[0].fecha.Day + " de " + fullMonthName + "  del " + venta[0].fecha.Year;
                //Identificador 1fact >1Guia
                //if (venta[0].Orden.GuiasSalida.Count >= 1)
                if (venta[0].Ordenes.FirstOrDefault().GuiasSalida.Count >= 1)
                {
                    string fechaTraslado = "";
                    //fechaTraslado = "Lima, " + venta[0].Orden.GuiasSalida.SingleOrDefault().fechaTraslado.Day + " de Agosto del" + venta[0].Orden.GuiasSalida.SingleOrDefault().fechaTraslado.Year;

                    _ravi["SerieG"] = venta[0].Ordenes.FirstOrDefault().GuiasSalida.SingleOrDefault().serie;
                    _ravi["NumeroG"] = venta[0].Ordenes.FirstOrDefault().GuiasSalida.SingleOrDefault().numero;
                   // _ravi["FechaG"] = venta[0].Orden.GuiasSalida.SingleOrDefault().fechaTraslado;
                }
                else
                {
                    _ravi["SerieG"] = "";
                    _ravi["NumeroG"] = "";
                    _ravi["FechaG"] = "";
                }
                _ravi["PuntoDestinoG"] = venta[0].Ordenes.FirstOrDefault().ClienteOrigen.Direcciones[0].Direccion.descripcion;
                _ravi["PuntoPartidaG"] = "CALLE AURELIO FERNANDEZ CONCHA 298-MIRAFLORES";
                _ravi["Cantidad"] = item.cantidad;
                _ravi["DescripcionP"] = item.Producto.nombre + " - " + item.Producto.UnidadMedida.nombre;
                _ravi["PrecioUnitario"] = string.Format("{0:0,0.00}", item.precioUnitario);
                _ravi["PrecioTotal"] = string.Format("{0:0,0.00}", item.precioTotal);

                ConvertirNumero conv = new ConvertirNumero();
                String numero = venta[0].Ordenes.FirstOrDefault().Total.ToString();
                _ravi["TotalTexto"] = conv.Convertir(numero, true);// "CIENTO OCHENTA Y OCHO y 10/100";

                dt.Rows.Add(_ravi);
            }


            Microsoft.Reporting.WebForms.LocalReport localreport = new LocalReport();
            ReportDataSource reportdatasource1 = new ReportDataSource("dsMaestroDetalle");
            reportdatasource1.Name = "dsMaestroDetalle";
            localreport.ReportPath = Server.MapPath("~/Reports/FacturaEmpty.rdlc");
            reportdatasource1.Value = dt;
            localreport.DataSources.Add(reportdatasource1);

            string reportType = "pdf";
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            string mimeType;
            renderedBytes = localreport.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }


        public FileResult DocumentBoletaEmptyPrint(int? id)
        {

            if (id == null)
            {
                return null;
            }
            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

            //1. Cambia Estado
            var pedidoOriginal = db.Ordenes.Find(id);
            var estadoPedido = db.EstadosOrden.Single(p => p.nombre == "Con Factura");

            pedidoOriginal.estadoOrdenId = estadoPedido.estadoOrdenId;
            pedidoOriginal.usuarioModificacion = User.Identity.Name;
            pedidoOriginal.fechaModificacion = cstTime;

            //2. Grabar Estado de Pedido
            var pedidoEstadoPedido = new OrdenEstadoOrden();

            pedidoEstadoPedido.ordenId = pedidoOriginal.ordenId;
            pedidoEstadoPedido.estadoOrdenId = estadoPedido.estadoOrdenId;
            pedidoEstadoPedido.usuarioCreacion = User.Identity.Name;
            pedidoEstadoPedido.fechaCreacion = cstTime;

            db.OrdenesEstadoOrden.Add(pedidoEstadoPedido);

            db.SaveChanges();



            List<Venta> venta = db.Ventas.Where(a => a.ordenId == id).ToList();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Cliente");
            dt.Columns.Add("Direccion");
            dt.Columns.Add("RUC");
            dt.Columns.Add("SerieF");
            dt.Columns.Add("NumeroF");
            dt.Columns.Add("SubTotal");
            dt.Columns.Add("IGV");
            dt.Columns.Add("Total");
            dt.Columns.Add("SerieG");
            dt.Columns.Add("NumeroG");
            dt.Columns.Add("FechaF");
            dt.Columns.Add("FechaG");
            dt.Columns.Add("PuntoDestinoG");
            dt.Columns.Add("PuntoPartidaG");
            dt.Columns.Add("Cantidad");
            dt.Columns.Add("DescripcionP");
            dt.Columns.Add("PrecioUnitario");
            dt.Columns.Add("PrecioTotal");
            dt.Columns.Add("TotalTexto");

            foreach (var item in venta[0].Ordenes.FirstOrDefault().Detalles.OrderBy(a => a.Producto.nombre))
            {
                DataRow _ravi = dt.NewRow();
                _ravi["Cliente"] = venta[0].Ordenes.FirstOrDefault().ClienteOrigen.razonSocial;
                _ravi["Direccion"] = venta[0].Ordenes.FirstOrDefault().ClienteOrigen.Direcciones[0].Direccion.descripcion;
                _ravi["RUC"] = venta[0].Ordenes.FirstOrDefault().ClienteOrigen.ruc;
                _ravi["SerieF"] = venta[0].serie;
                _ravi["NumeroF"] = venta[0].numero;
                _ravi["SubTotal"] = string.Format("{0:0,0.00}", venta[0].Ordenes.FirstOrDefault().subTotal);
                _ravi["IGV"] = string.Format("{0:0,0.00}", venta[0].Ordenes.FirstOrDefault().igv);

                _ravi["Total"] = string.Format("{0:0,0.00}", venta[0].Ordenes.FirstOrDefault().Total);

                string fullMonthName = venta[0].fecha.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper();
                _ravi["FechaF"] = "Lima, " + venta[0].fecha.Day + " de " + fullMonthName + "  del " + venta[0].fecha.Year;

                if (venta[0].Ordenes.FirstOrDefault().GuiasSalida.Count >= 1)
                {
                    string fechaTraslado = "";
                    //fechaTraslado = "Lima, " + venta[0].Orden.GuiasSalida.SingleOrDefault().fechaTraslado.Day + " de Agosto del" + venta[0].Orden.GuiasSalida.SingleOrDefault().fechaTraslado.Year;

                    _ravi["SerieG"] = venta[0].Ordenes.FirstOrDefault().GuiasSalida.SingleOrDefault().serie;
                    _ravi["NumeroG"] = venta[0].Ordenes.FirstOrDefault().GuiasSalida.SingleOrDefault().numero;
                    //_ravi["FechaG"] = venta[0].Orden.GuiasSalida.SingleOrDefault().fechaTraslado;
                }
                else
                {
                    _ravi["SerieG"] = "";
                    _ravi["NumeroG"] = "";
                    _ravi["FechaG"] = "";
                }
                _ravi["PuntoDestinoG"] = venta[0].Ordenes.FirstOrDefault().ClienteOrigen.Direcciones[0].Direccion.descripcion;
                _ravi["PuntoPartidaG"] = "CALLE AURELIO FERNANDEZ CONCHA 298-MIRAFLORES";
                _ravi["Cantidad"] = item.cantidad;
                _ravi["DescripcionP"] = item.Producto.nombre + " - " + item.Producto.UnidadMedida.nombre;
                _ravi["PrecioUnitario"] = string.Format("{0:0,0.00}", item.precioUnitario);
                _ravi["PrecioTotal"] = string.Format("{0:0,0.00}", item.precioTotal);
                _ravi["TotalTexto"] = "CIENTO OCHENTA Y OCHO y 10/100";

                dt.Rows.Add(_ravi);
            }


            Microsoft.Reporting.WebForms.LocalReport localreport = new LocalReport();
            ReportDataSource reportdatasource1 = new ReportDataSource("dsMaestroDetalle");
            reportdatasource1.Name = "dsMaestroDetalle";
            localreport.ReportPath = Server.MapPath("~/Reports/BoletaEmpty.rdlc");
            reportdatasource1.Value = dt;
            localreport.DataSources.Add(reportdatasource1);

            string reportType = "pdf";
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            string mimeType;
            renderedBytes = localreport.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }


    }
}
