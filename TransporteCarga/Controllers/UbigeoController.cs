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
    public class UbigeoController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /Ubigeo/
        public ActionResult Index()
        {
            return View(db.Ubigeo.ToList());
        }

        // GET: /Ubigeo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ubigeo ubigeo = db.Ubigeo.Find(id);
            if (ubigeo == null)
            {
                return HttpNotFound();
            }
            return View(ubigeo);
        }

        // GET: /Ubigeo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Ubigeo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ubigeoId,codigo,descripcion")] Ubigeo ubigeo)
        {
            if (ModelState.IsValid)
            {
                db.Ubigeo.Add(ubigeo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ubigeo);
        }

        // GET: /Ubigeo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ubigeo ubigeo = db.Ubigeo.Find(id);
            if (ubigeo == null)
            {
                return HttpNotFound();
            }
            return View(ubigeo);
        }

        // POST: /Ubigeo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ubigeoId,codigo,descripcion")] Ubigeo ubigeo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ubigeo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ubigeo);
        }

        // GET: /Ubigeo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ubigeo ubigeo = db.Ubigeo.Find(id);
            if (ubigeo == null)
            {
                return HttpNotFound();
            }
            return View(ubigeo);
        }

        // POST: /Ubigeo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ubigeo ubigeo = db.Ubigeo.Find(id);
            db.Ubigeo.Remove(ubigeo);
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

        // GET: /Envio/Details/5
        public JsonResult GetProvincias(string codigo)
        {
            //var provincias = new SelectList(db.Ubigeo.OrderBy(a => a.descripcion).Where(a => a.codigo.Substring(0, 2) == codigo && a.codigo.Substring(2, 2) != "00" && a.codigo.Substring(4, 2) == "01"), "codigo", "descripcion");
            var provincias =  db.Ubigeo.OrderBy(a => a.descripcion).Where(a => a.codigo.Substring(0, 2) == codigo && a.codigo.Substring(2, 2) != "00" && a.codigo.Substring(4, 2) == "01").ToList();
            //where substring(codigo,1,2) = '10' and substring(codigo,3,2) <> '00' and substring(codigo,5,2) = '01'
            return new JsonResult { Data = provincias, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: /Envio/Details/5
        public JsonResult GetDistritos(string codigo)
        {           
            var distritos = db.Ubigeo.OrderBy(a => a.descripcion).Where(a => a.codigo.Substring(0, 4) == codigo).ToList();
            return new JsonResult { Data = distritos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
