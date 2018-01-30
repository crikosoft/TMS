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
    public class UnidadMedidaController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /UnidadMedida/
        public ActionResult Index()
        {
            return View(db.UnidadMedida.ToList());
        }

        // GET: /UnidadMedida/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadMedida unidadmedida = db.UnidadMedida.Find(id);
            if (unidadmedida == null)
            {
                return HttpNotFound();
            }
            return View(unidadmedida);
        }

        // GET: /UnidadMedida/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /UnidadMedida/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="unidadMedidaId,nombre,sigla")] UnidadMedida unidadmedida)
        {
            if (ModelState.IsValid)
            {
                db.UnidadMedida.Add(unidadmedida);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unidadmedida);
        }

        // GET: /UnidadMedida/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadMedida unidadmedida = db.UnidadMedida.Find(id);
            if (unidadmedida == null)
            {
                return HttpNotFound();
            }
            return View(unidadmedida);
        }

        // POST: /UnidadMedida/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="unidadMedidaId,nombre,sigla")] UnidadMedida unidadmedida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unidadmedida).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unidadmedida);
        }

        // GET: /UnidadMedida/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadMedida unidadmedida = db.UnidadMedida.Find(id);
            if (unidadmedida == null)
            {
                return HttpNotFound();
            }
            return View(unidadmedida);
        }

        // POST: /UnidadMedida/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UnidadMedida unidadmedida = db.UnidadMedida.Find(id);
            db.UnidadMedida.Remove(unidadmedida);
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
