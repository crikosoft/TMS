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
    public class NotaCreditoController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /NotaCredito/
        public ActionResult Index()
        {
            var notacredito = db.NotaCredito.Include(n => n.NotaCreditoConcepto).Include(n => n.Venta);
            return View(notacredito.ToList());
        }

        // GET: /NotaCredito/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotaCredito notacredito = db.NotaCredito.Find(id);
            if (notacredito == null)
            {
                return HttpNotFound();
            }
            return View(notacredito);
        }

        // GET: /NotaCredito/Create
        public ActionResult Create()
        {

            var queryDocumentoVenta = from q in db.Ventas
                                select new
                                {
                                    ventaId = q.ventaId,
                                    numero = q.serie + "-" + q.numero
                                };

            ViewBag.notaCreditoConceptoId = new SelectList(db.NotaCreditoConcepto, "notaCreditoConceptoId", "concepto");
            ViewBag.ventaId = new SelectList(queryDocumentoVenta, "ventaId", "numero");
            return View();
        }

        // POST: /NotaCredito/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="notaCreditoId,ventaId,monto,notaCreditoConceptoId,numero,usuarioCreacion,usuarioModificacion,fechaCreacion,fechaModificacion")] NotaCredito notacredito)
        {
            if (ModelState.IsValid)
            {

                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);


                notacredito.fechaCreacion = cstTime;
                notacredito.usuarioCreacion = User.Identity.Name;
                notacredito.fechaModificacion = cstTime;
                notacredito.usuarioModificacion = User.Identity.Name;

                db.NotaCredito.Add(notacredito);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.notaCreditoConceptoId = new SelectList(db.NotaCreditoConcepto, "notaCreditoConceptoId", "concepto", notacredito.notaCreditoConceptoId);
            ViewBag.ventaId = new SelectList(db.Ventas, "ventaId", "serie", notacredito.ventaId);
            return View(notacredito);
        }

        // GET: /NotaCredito/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotaCredito notacredito = db.NotaCredito.Find(id);
            if (notacredito == null)
            {
                return HttpNotFound();
            }

            var queryDocumentoVenta = from q in db.Ventas
                                      select new
                                      {
                                          ventaId = q.ventaId,
                                          numero = q.serie + "-" + q.numero
                                      };

            ViewBag.notaCreditoConceptoId = new SelectList(db.NotaCreditoConcepto, "notaCreditoConceptoId", "concepto", notacredito.notaCreditoConceptoId);
            ViewBag.ventaId = new SelectList(queryDocumentoVenta, "ventaId", "numero", notacredito.ventaId);
            return View(notacredito);
        }

        // POST: /NotaCredito/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="notaCreditoId,ventaId,monto,notaCreditoConceptoId,numero,usuarioCreacion,usuarioModificacion,fechaCreacion,fechaModificacion")] NotaCredito notacredito)
        {
            if (ModelState.IsValid)
            {

                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

                notacredito.fechaModificacion = cstTime;
                notacredito.usuarioModificacion = User.Identity.Name;

                db.Entry(notacredito).State = EntityState.Modified;
                db.Entry(notacredito).Property(x => x.fechaCreacion).IsModified = false;
                db.Entry(notacredito).Property(x => x.usuarioCreacion).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }



            ViewBag.notaCreditoConceptoId = new SelectList(db.NotaCreditoConcepto, "notaCreditoConceptoId", "concepto", notacredito.notaCreditoConceptoId);
            ViewBag.ventaId = new SelectList(db.Ventas, "ventaId", "serie", notacredito.ventaId);
            return View(notacredito);
        }

        // GET: /NotaCredito/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotaCredito notacredito = db.NotaCredito.Find(id);
            if (notacredito == null)
            {
                return HttpNotFound();
            }
            return View(notacredito);
        }

        // POST: /NotaCredito/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NotaCredito notacredito = db.NotaCredito.Find(id);
            db.NotaCredito.Remove(notacredito);
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
