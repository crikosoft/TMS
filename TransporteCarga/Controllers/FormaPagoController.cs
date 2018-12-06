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
    public class FormaPagoController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /FormaPago/
        public ActionResult Index()
        {
            return View(db.FormaPagos.ToList());
        }

        // GET: /FormaPago/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormaPago formapago = db.FormaPagos.Find(id);
            if (formapago == null)
            {
                return HttpNotFound();
            }
            return View(formapago);
        }

        // GET: /FormaPago/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /FormaPago/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="formaPagoId,nombre,descripcion")] FormaPago formapago)
        {
            if (ModelState.IsValid)
            {
                db.FormaPagos.Add(formapago);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(formapago);
        }

        // GET: /FormaPago/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormaPago formapago = db.FormaPagos.Find(id);
            if (formapago == null)
            {
                return HttpNotFound();
            }
            return View(formapago);
        }

        // POST: /FormaPago/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="formaPagoId,nombre,descripcion")] FormaPago formapago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formapago).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(formapago);
        }

        // GET: /FormaPago/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormaPago formapago = db.FormaPagos.Find(id);
            if (formapago == null)
            {
                return HttpNotFound();
            }
            return View(formapago);
        }

        // POST: /FormaPago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FormaPago formapago = db.FormaPagos.Find(id);
            db.FormaPagos.Remove(formapago);
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
