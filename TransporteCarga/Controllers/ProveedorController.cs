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
    public class ProveedorController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /Proveedor/
        public ActionResult Index()
        {
            var proveedores = db.Proveedores.Include(p => p.Banco);
            return View(proveedores.ToList());
        }

        // GET: /Proveedor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // GET: /Proveedor/Create
        public ActionResult Create()
        {
            ViewBag.bancoId = new SelectList(db.Bancos, "bancoId", "nombre");
            return View();
        }

        // POST: /Proveedor/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="proveedorId,razonSocial,contacto,ruc,direccion,telefono,email,bancoId,nroCuenta")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                db.Proveedores.Add(proveedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.bancoId = new SelectList(db.Bancos, "bancoId", "nombre", proveedor.bancoId);
            return View(proveedor);
        }

        // GET: /Proveedor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            ViewBag.bancoId = new SelectList(db.Bancos, "bancoId", "nombre", proveedor.bancoId);
            return View(proveedor);
        }

        // POST: /Proveedor/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="proveedorId,razonSocial,contacto,ruc,direccion,telefono,email,bancoId,nroCuenta")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

                proveedor.fechaModificacion = cstTime;
                proveedor.usuarioModificacion = User.Identity.Name;

                db.Entry(proveedor).State = EntityState.Modified;
                db.Entry(proveedor).Property(x => x.fechaCreacion).IsModified = false;
                db.Entry(proveedor).Property(x => x.usuarioCreacion).IsModified = false;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.bancoId = new SelectList(db.Bancos, "bancoId", "nombre", proveedor.bancoId);
            return View(proveedor);
        }

        // GET: /Proveedor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // POST: /Proveedor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proveedor proveedor = db.Proveedores.Find(id);
            db.Proveedores.Remove(proveedor);
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
