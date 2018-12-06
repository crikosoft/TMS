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
    public class EnvioController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /Envio/
        public ActionResult Index()
        {
            var envios = db.Envios.Include(e => e.Chofer).Include(e => e.Proveedor).Include(e => e.Vehiculo).OrderByDescending(a=>a.envioId);
            return View(envios.ToList());
        }

        public ActionResult IndexRentabilidad(DateTime? start, DateTime? end)
        {
            ViewBag.start = start;
            ViewBag.end = end;

            List<Envio> envios = new List<Envio>();
            if (start != null && end != null)
            {
                //                pedidos = db.Pedidos.Where(a => a.EstadoPedido.nombre!= "Anulado").Where(a => DbFunctions.TruncateTime(a.fechaCreacion) >= start && DbFunctions.TruncateTime(a.fechaCreacion) <= end).ToList();
                //envios = db.Envios.Where(a => a.EstadoEnvio.nombre != "Anulado").Where(a => DbFunctions.TruncateTime(a.Ventas.FirstOrDefault().fecha) >= start && DbFunctions.TruncateTime(a.Ventas.FirstOrDefault().fecha) <= end).ToList();
                envios = db.Envios.Where(a => a.EstadoEnvio.nombre != "Anulado").Where(a => DbFunctions.TruncateTime(a.fechaTraslado) >= start && DbFunctions.TruncateTime(a.fechaTraslado) <= end).ToList();
            }
            else
            {
                envios = db.Envios.Where(a => a.EstadoEnvio.nombre != "Anulado").ToList();
            }

            return View(envios.ToList());
        }

        public ActionResult IndexReducedPayPending(DateTime? start, DateTime? end)
        {
            ViewBag.start = start;
            ViewBag.end = end;

            List<Envio> envios = new List<Envio>();
            if (start != null && end != null)
            {

                envios = db.Envios.Where(a => a.EstadoEnvio.nombre == "Creado").Where(a => DbFunctions.TruncateTime(a.fechaPagoProgramado) >= start && DbFunctions.TruncateTime(a.fechaPagoProgramado) <= end).ToList();
            }
            else
            {
                envios = db.Envios.Where(a => a.EstadoEnvio.nombre == "Creado").ToList();
            }

            return View(envios.ToList());
        }


        public ActionResult IndexPayPending()
        {
            var estadoList = new string[] { "Creado", "Entregado"};
            var envios = db.Envios.Where(o => estadoList.Contains(o.EstadoEnvio.nombre)).Include(o => o.FormaPago);


            ViewBag.estadoEnvioId = new SelectList(db.EstadoEnvios.Where(o => estadoList.Contains(o.nombre)), "nombre", "nombre");
            ViewBag.proveedorId = new SelectList(db.Proveedores.OrderBy(a => a.razonSocial), "razonSocial", "razonSocial");

            return View(envios.ToList());

        }


        // GET: /Envio/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Envio envio = db.Envios.Find(id);
            if (envio == null)
            {
                return HttpNotFound();
            }
            return View(envio);
        }

        // GET: /Envio/Details/5
        public JsonResult GetChoferes(int? id)
        {

            var queryChoferes = from q in db.Choferes
                                .Where(a=>a.proveedorId==id)
                                select new
                                {
                                    choferId = q.choferId,
                                    nombres = q.apellidos + ", " + q.nombres
                                }
                                ;

            //ViewBag.choferId = new SelectList(queryChoferes, "choferId", "nombres");

            //List<Chofer> choferes = db.Choferes.Where(a => a.proveedorId ==id).ToList();
            //List<Chofer> choferes = db.Choferes.Where(a => a.proveedorId == id).ToList();
            ViewBag.choferId = queryChoferes;
            return new JsonResult { Data = ViewBag.choferId, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: /Envio/Details/5
        public JsonResult GetVehiculos(int? id)
        {
            List<Vehiculo> vehiculos = db.Vehiculo.Where(a => a.proveedorId == id).ToList();
            ViewBag.vehiculoId = vehiculos;
            return new JsonResult { Data = ViewBag.vehiculoId, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: /Envio/Create
        public ActionResult Create()
        {

            var queryChoferes = from q in db.Choferes
                                   select new
                                   {
                                       choferId = q.choferId,
                                       nombres = q.apellidos + ", " + q.nombres
                                   };

            ViewBag.choferId = new SelectList(queryChoferes.OrderBy(a => a.nombres), "choferId", "nombres");


            //ViewBag.choferId = new SelectList(db.Choferes, "choferId", "nombres");
            ViewBag.proveedorId = new SelectList(db.Proveedores.OrderBy(a=>a.razonSocial), "proveedorId", "razonSocial");
            ViewBag.vehiculoId = new SelectList(db.Vehiculo, "vehiculoId", "placaUnidad");
            ViewBag.formaPagoId = new SelectList(db.FormaPagos, "formaPagoId", "nombre");
            ViewBag.Ordenes = db.Ordenes.Where(a=> a.Envios.Count()==0).Include(e => e.ClientePago).Include(e => e.DireccionOrigen).Include(e => e.DireccionDestino).Include(e=>e.GuiasSalida);
            

            Envio model = new Envio
            {
                OrdenIds = new int[0]
            };

            return View();
        }

        // POST: /Envio/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "envioId,proveedorId,choferId,vehiculoId,subTotal,igv,Total,formaPagoId,comentario,usuarioCreacion,usuarioModificacion,fechaCreacion,fechaModificacion,fechaTraslado, OrdenIds")] Envio envio)
        {
            if (ModelState.IsValid)
            {
                if (envio.OrdenIds != null)
                {
                    envio.Ordenes = db.Ordenes.Where(a => envio.OrdenIds.Contains(a.ordenId)).ToList();
                }

                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

                var estadoEnvio = db.EstadoEnvios.Single(p => p.nombre == "Creado");
                envio.estadoEnvioId = estadoEnvio.estadoEnvioId;
                envio.igv = envio.subTotal * 0.18;
                envio.Total = envio.subTotal * 1.18;

                if (envio.formaPagoId != null)
                { 
                    var formaPago = db.FormaPagos.Find(envio.formaPagoId);

                    switch (formaPago.nombre)
                    {
                        case "Factura a 7 días":
                             envio.fechaPagoProgramado = envio.fechaTraslado.AddDays(7);
                            break;
                        case "Factura a 15 días":
                             envio.fechaPagoProgramado = envio.fechaTraslado.AddDays(15);
                            break;
                        case "Factura a 30 días":
                             envio.fechaPagoProgramado = envio.fechaTraslado.AddDays(30);
                            break;
                        case "Factura a 90 días":
                             envio.fechaPagoProgramado = envio.fechaTraslado.AddDays(90);
                            break;
                        default:
                              envio.fechaPagoProgramado = envio.fechaTraslado;
                              break;
                    }
                }
                envio.fechaCreacion = cstTime;
                envio.usuarioCreacion = User.Identity.Name;
                envio.fechaModificacion = cstTime;
                envio.usuarioModificacion = User.Identity.Name;

                db.Envios.Add(envio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var queryChoferes = from q in db.Choferes
                                select new
                                {
                                    choferId = q.choferId,
                                    nombres = q.apellidos + ", " + q.nombres
                                };

            ViewBag.choferId = new SelectList(queryChoferes.OrderBy(a=>a.nombres), "choferId", "nombres");

            ViewBag.proveedorId = new SelectList(db.Proveedores.OrderBy(a=>a.razonSocial), "proveedorId", "razonSocial");
            ViewBag.vehiculoId = new SelectList(db.Vehiculo, "vehiculoId", "placaUnidad");
            ViewBag.Ordenes = db.Ordenes.Where(a => a.Envios.Count() == 0).Include(e => e.ClientePago).Include(e => e.DireccionOrigen).Include(e => e.DireccionDestino);
            ViewBag.formaPagoId = new SelectList(db.FormaPagos, "formaPagoId", "nombre");

            Envio model = new Envio
            {
                OrdenIds = new int[0]
            };

            return View(envio);
        }

        // GET: /Envio/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Envio envio = db.Envios.Where(a=>a.envioId == id).Include(a=>a.Ordenes).SingleOrDefault();
            if (envio == null)
            {
                return HttpNotFound();
            }

            var queryChoferes = from q in db.Choferes
                                select new
                                {
                                    choferId = q.choferId,
                                    nombres = q.apellidos + ", " + q.nombres
                                };

            ViewBag.choferId = new SelectList(queryChoferes.OrderBy(a => a.nombres), "choferId", "nombres", envio.choferId);

            //ViewBag.choferId = new SelectList(db.Choferes, "choferId", "nombres", envio.choferId);
            ViewBag.proveedorId = new SelectList(db.Proveedores.OrderBy(a=>a.razonSocial), "proveedorId", "razonSocial", envio.proveedorId);
            ViewBag.vehiculoId = new SelectList(db.Vehiculo, "vehiculoId", "placaUnidad", envio.vehiculoId);
            ViewBag.Ordenes = db.Ordenes.Where(a => a.Envios.Count() == 0 || a.Envios.Any(b => b.envioId == id)).Include(e => e.ClientePago).Include(e => e.DireccionOrigen).Include(e => e.DireccionDestino).Include(e => e.GuiasSalida); 
            ViewBag.formaPagoId = new SelectList(db.FormaPagos, "formaPagoId", "nombre", envio.formaPagoId);

            Envio model = new Envio
            {
                OrdenIds = new int[0]
            };

            return View(envio);
        }

        // POST: /Envio/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "envioId,proveedorId,choferId,vehiculoId,subTotal,igv,Total,formaPagoId,comentario,usuarioCreacion,usuarioModificacion,fechaCreacion,fechaModificacion,fechaTraslado, OrdenIds")] Envio envio)
        {
            if (ModelState.IsValid)
            {
                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);



                //db.Configuration.LazyLoadingEnabled = false;


                var envioOriginal = db.Envios.Find(envio.envioId);
//                var ordenesOriginal = db.Ordenes.Where(a => a.Envios.FirstOrDefault().envioId == envio.envioId).Include(b=>b.Envios);
//                envio.Ordenes = ordenesOriginal.ToList();
                envioOriginal.Ordenes.RemoveAll(a => a.Envios.SingleOrDefault().envioId == envioOriginal.envioId);
//                envio.Ordenes.RemoveAll(a => a.Envios.SingleOrDefault().envioId == envio.envioId);
                
                //db.SaveChanges();

                if (envio.OrdenIds != null)
                {
                    var ordenes = db.Ordenes.Where(a => envio.OrdenIds.Contains(a.ordenId)).ToList();
                    foreach (var item in ordenes)
                    {
                        envioOriginal.Ordenes.Add(item);
                    }
                }

                envioOriginal.subTotal = envio.subTotal;
                envioOriginal.igv = envio.subTotal * 0.18;
                envioOriginal.Total = envio.subTotal * 1.18;
                envioOriginal.proveedorId = envio.proveedorId;
                envioOriginal.choferId = envio.choferId;
                envioOriginal.vehiculoId = envio.vehiculoId;
                envioOriginal.formaPagoId = envio.formaPagoId;
                //envioOriginal.fechaPagoProgramado = envio.fechaPagoProgramado;
                envioOriginal.comentario = envio.comentario;
                envioOriginal.fechaModificacion =cstTime;
                envioOriginal.usuarioModificacion = User.Identity.Name;

                if (envio.formaPagoId != null)
                {
                
                    var formaPago = db.FormaPagos.Find(envio.formaPagoId);

                    switch (formaPago.nombre)
                    {
                        case "Factura a 7 días":
                            envioOriginal.fechaPagoProgramado = envio.fechaTraslado.AddDays(7);
                            break;
                        case "Factura a 15 días":
                            envioOriginal.fechaPagoProgramado = envio.fechaTraslado.AddDays(15);
                            break;
                        case "Factura a 30 días":
                            envioOriginal.fechaPagoProgramado = envio.fechaTraslado.AddDays(30);
                            break;
                        case "Factura a 90 días":
                            envioOriginal.fechaPagoProgramado = envio.fechaTraslado.AddDays(90);
                            break;
                        default:
                            envioOriginal.fechaPagoProgramado = envio.fechaTraslado;
                            break;
                    }
                }


                //db.Entry(envio).CurrentValues.SetValues(envio);              

                //db.Entry(envio).State = EntityState.Detached;
                //db.Entry(envio).Property(x => x.estadoEnvioId).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var queryChoferes = from q in db.Choferes
                                select new
                                {
                                    choferId = q.choferId,
                                    nombres = q.apellidos + ", " + q.nombres
                                };

            ViewBag.choferId = new SelectList(queryChoferes.OrderBy(a => a.nombres), "choferId", "nombres", envio.choferId);


            //ViewBag.choferId = new SelectList(db.Choferes, "choferId", "nombres", envio.choferId);
            ViewBag.proveedorId = new SelectList(db.Proveedores.OrderBy(a => a.razonSocial), "proveedorId", "razonSocial", envio.proveedorId);
            ViewBag.vehiculoId = new SelectList(db.Vehiculo, "vehiculoId", "placaUnidad", envio.vehiculoId);
            ViewBag.formaPagoId = new SelectList(db.FormaPagos, "formaPagoId", "nombre", envio.formaPagoId);
            ViewBag.Ordenes = db.Ordenes.Where(a => a.Envios.Count() == 0 || a.Envios.Any(b => b.envioId == envio.envioId)).Include(e => e.ClientePago).Include(e => e.DireccionOrigen).Include(e => e.DireccionDestino);

            Envio model = new Envio
            {
                OrdenIds = new int[0]
            };
            return View(envio);
        }

        // GET: /Envio/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Envio envio = db.Envios.Find(id);
            if (envio == null)
            {
                return HttpNotFound();
            }
            return View(envio);
        }

        // POST: /Envio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Envio envio = db.Envios.Find(id);
            db.Envios.Remove(envio);
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
