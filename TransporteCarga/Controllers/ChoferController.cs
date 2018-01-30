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
    public class ChoferController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /Chofer/
        public ActionResult Index()
        {
            var choferes = db.Choferes.Include(c => c.Proveedor);
            return View(choferes.ToList());
        }

        // GET: /Chofer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chofer chofer = db.Choferes.Find(id);
            if (chofer == null)
            {
                return HttpNotFound();
            }
            return View(chofer);
        }

        // GET: /Chofer/Create
        public ActionResult Create()
        {
            ViewBag.proveedorId = new SelectList(db.Proveedores.OrderBy(a => a.razonSocial), "proveedorId", "razonSocial");
            return View();
        }

        // POST: /Chofer/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="choferId,nombres,apellidos,nroDocumentoIdentidad,tipoDocumentoIdentodadId,fechaNacimiento,numeroBrevete,proveedorId,usuarioCreacion,usuarioModificacion,fechaCreacion,fechaModificacion")] Chofer chofer)
        {
            if (ModelState.IsValid)
            {
                db.Choferes.Add(chofer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.proveedorId = new SelectList(db.Proveedores.OrderBy(a => a.razonSocial), "proveedorId", "razonSocial", chofer.proveedorId);
            return View(chofer);
        }

        // GET: /Chofer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chofer chofer = db.Choferes.Find(id);
            if (chofer == null)
            {
                return HttpNotFound();
            }
            ViewBag.proveedorId = new SelectList(db.Proveedores.OrderBy(a => a.razonSocial), "proveedorId", "razonSocial", chofer.proveedorId);
            return View(chofer);
        }

        // POST: /Chofer/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="choferId,nombres,apellidos,nroDocumentoIdentidad,tipoDocumentoIdentodadId,fechaNacimiento,numeroBrevete,proveedorId")] Chofer chofer)
        {
            if (ModelState.IsValid)
            {

                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

                chofer.fechaModificacion = cstTime;
                chofer.usuarioModificacion = User.Identity.Name;

                db.Entry(chofer).State = EntityState.Modified;
                db.Entry(chofer).Property(x => x.fechaCreacion).IsModified = false;
                db.Entry(chofer).Property(x => x.usuarioCreacion).IsModified = false;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.proveedorId = new SelectList(db.Proveedores.OrderBy(a => a.razonSocial), "proveedorId", "razonSocial", chofer.proveedorId);
            return View(chofer);
        }

        // GET: /Chofer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chofer chofer = db.Choferes.Find(id);
            if (chofer == null)
            {
                return HttpNotFound();
            }
            return View(chofer);
        }

        // POST: /Chofer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chofer chofer = db.Choferes.Find(id);
            db.Choferes.Remove(chofer);
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
