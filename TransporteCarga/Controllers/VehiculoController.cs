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
    public class VehiculoController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /Vehiculo/
        public ActionResult Index()
        {
            var vehiculo = db.Vehiculo.Include(v => v.Proveedor);
            return View(vehiculo.ToList());
        }

        // GET: /Vehiculo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehiculo vehiculo = db.Vehiculo.Find(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(vehiculo);
        }

        // GET: /Vehiculo/Create
        public ActionResult Create()
        {
            ViewBag.proveedorId = new SelectList(db.Proveedores.OrderBy(a => a.razonSocial), "proveedorId", "razonSocial");
            return View();
        }

        // POST: /Vehiculo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="vehiculoId,placaUnidad,placaCarretera,configuracionVehicular,certificadoInscripcion1,certificadoInscripcion2,Marca,Modelo,proveedorId,usuarioCreacion,usuarioModificacion,fechaCreacion,fechaModificacion")] Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                db.Vehiculo.Add(vehiculo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.proveedorId = new SelectList(db.Proveedores.OrderBy(a=>a.razonSocial), "proveedorId", "razonSocial", vehiculo.proveedorId);
            return View(vehiculo);
        }

        // GET: /Vehiculo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehiculo vehiculo = db.Vehiculo.Find(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            ViewBag.proveedorId = new SelectList(db.Proveedores.OrderBy(a => a.razonSocial), "proveedorId", "razonSocial", vehiculo.proveedorId);
            return View(vehiculo);
        }

        // POST: /Vehiculo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="vehiculoId,placaUnidad,placaCarretera,configuracionVehicular,certificadoInscripcion1,certificadoInscripcion2,Marca,Modelo,proveedorId")] Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {

                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

                vehiculo.fechaModificacion = cstTime;
                vehiculo.usuarioModificacion = User.Identity.Name;

                db.Entry(vehiculo).State = EntityState.Modified;
                db.Entry(vehiculo).Property(x => x.fechaCreacion).IsModified = false;
                db.Entry(vehiculo).Property(x => x.usuarioCreacion).IsModified = false;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.proveedorId = new SelectList(db.Proveedores.OrderBy(a => a.razonSocial), "proveedorId", "razonSocial", vehiculo.proveedorId);
            return View(vehiculo);
        }

        // GET: /Vehiculo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehiculo vehiculo = db.Vehiculo.Find(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(vehiculo);
        }

        // POST: /Vehiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehiculo vehiculo = db.Vehiculo.Find(id);
            db.Vehiculo.Remove(vehiculo);
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
