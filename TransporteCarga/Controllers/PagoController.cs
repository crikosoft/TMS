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
    public class PagoController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /Pago/
        public ActionResult Index()
        {

            ViewBag.estadoPagoId = new SelectList(db.EstadoPagos, "nombre", "nombre");
            ViewBag.proveedorId = new SelectList(db.Proveedores.OrderBy(a => a.razonSocial), "razonSocial", "razonSocial");

            var pagos = db.Pagos.Include(p => p.Envio).Include(p => p.EstadoPago).Include(p => p.TipoDocumento);
            return View(pagos.ToList());
        }

        // GET: /Pago/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = db.Pagos.Find(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            return View(pago);
        }

        // GET: /Pago/Create
        public ActionResult Create(int? id)
        {

            Pago pago = new Pago();

            Envio Envio = db.Envios.Where(a => a.envioId == id).FirstOrDefault();
            pago.envioId = Envio.envioId;
            pago.Envio = Envio;

            var tipoDocumentoList = new string[] { "Factura de Compra", "Boleta de Compra", "Voucher" };

            ViewBag.estadoPagoId = new SelectList(db.EstadoPagos, "estadoPagoId", "nombre");
            ViewBag.tipoDocumentoId = new SelectList(db.TipoDocumentos.Where(p => tipoDocumentoList.Contains(p.nombre)), "tipoDocumentoId", "nombre");

            return View(pago);

            //ViewBag.envioId = new SelectList(db.Envios, "envioId", "comentario");
            //ViewBag.estadoPagoId = new SelectList(db.EstadoPagos, "estadoPagoId", "nombre");
            //ViewBag.tipoDocumentoId = new SelectList(db.TipoDocumentos, "tipoDocumentoId", "nombre");
            //return View();
        }

        // POST: /Pago/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="pagoId,envioId,tipoDocumentoId,numero,fechaPagoReal,estadoPagoId,pagoMonto,pagoDetraccion,asientoContable,fechaContable,fechaFactura,usuarioCreacion,usuarioModificacion,fechaCreacion,fechaModificacion,preguntaPagoTotal")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

                
                //Estado de Envío
                if (pago.preguntaPagoTotal == true) //2: Pendiente de Pago
                {
                    var paramtext = "Pagado";
                    var estadoEnvio = db.EstadoEnvios.Single(p => p.nombre == paramtext);
                    var envio = db.Envios.Find(pago.envioId);
                    envio.estadoEnvioId = estadoEnvio.estadoEnvioId;
                    pago.Envio = envio;
                }

                // Calculo de Detracción
                if (pago.pagoMonto != null)
                {
                    pago.pagoDetraccion = 0.04 * pago.pagoMonto;
                    pago.pagoDetraccion = Math.Round((double)pago.pagoDetraccion, MidpointRounding.AwayFromZero);
                }


                pago.fechaCreacion = cstTime;
                pago.usuarioCreacion = User.Identity.Name;

                db.Pagos.Add(pago);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.envioId = new SelectList(db.Envios, "envioId", "comentario", pago.envioId);
            ViewBag.estadoPagoId = new SelectList(db.EstadoPagos, "estadoPagoId", "nombre", pago.estadoPagoId);
            ViewBag.tipoDocumentoId = new SelectList(db.TipoDocumentos, "tipoDocumentoId", "nombre", pago.tipoDocumentoId);
            return View(pago);
        }

        // GET: /Pago/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = db.Pagos.Find(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            ViewBag.envioId = new SelectList(db.Envios, "envioId", "comentario", pago.envioId);
            ViewBag.estadoPagoId = new SelectList(db.EstadoPagos, "estadoPagoId", "nombre", pago.estadoPagoId);
            ViewBag.tipoDocumentoId = new SelectList(db.TipoDocumentos, "tipoDocumentoId", "nombre", pago.tipoDocumentoId);
            return View(pago);
        }

        // POST: /Pago/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="pagoId,envioId,tipoDocumentoId,numero,fechaPagoReal,estadoPagoId,pagoMonto,pagoDetraccion,asientoContable,fechaContable,fechaFactura,usuarioCreacion,usuarioModificacion,fechaCreacion,fechaModificacion,preguntaPagoTotal")] Pago pago)
        {
            if (ModelState.IsValid)
            {

                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
                
                pago.fechaModificacion = cstTime;
                pago.usuarioModificacion = User.Identity.Name;

                //Estado de Envío
                if (pago.preguntaPagoTotal == true) 
                {
                    var paramtext = "Pagado";
                    var estadoEnvio = db.EstadoEnvios.Single(p => p.nombre == paramtext);
                    var envio = db.Envios.Find(pago.envioId);
                    envio.estadoEnvioId = estadoEnvio.estadoEnvioId;
                    pago.Envio = envio;
                }

                // Calculo de Detracción
                if (pago.pagoMonto != null)
                {
                    pago.pagoDetraccion = 0.04 * pago.pagoMonto;
                    pago.pagoDetraccion = Math.Round((double)pago.pagoDetraccion, MidpointRounding.AwayFromZero);
                }

                db.Entry(pago).State = EntityState.Modified;
                db.Entry(pago).Property(x => x.fechaCreacion).IsModified = false;
                db.Entry(pago).Property(x => x.usuarioCreacion).IsModified = false;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.envioId = new SelectList(db.Envios, "envioId", "comentario", pago.envioId);
            ViewBag.estadoPagoId = new SelectList(db.EstadoPagos, "estadoPagoId", "nombre", pago.estadoPagoId);
            ViewBag.tipoDocumentoId = new SelectList(db.TipoDocumentos, "tipoDocumentoId", "nombre", pago.tipoDocumentoId);
            return View(pago);
        }

        // GET: /Pago/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = db.Pagos.Find(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            return View(pago);
        }

        // POST: /Pago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pago pago = db.Pagos.Find(id);
            db.Pagos.Remove(pago);
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
