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
    public class DireccionController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /Direccion/
        public ActionResult Index()
        {
            var direcciones = db.Direcciones.Include(d => d.Ubigeo);
            return View(direcciones.ToList());
        }

        // GET: /Direccion/
        public ActionResult Map()
        {
           
            return View();
        }


        // GET: /Direccion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Direccion direccion = db.Direcciones.Find(id);
            if (direccion == null)
            {
                return HttpNotFound();
            }
            return View(direccion);
        }

        // GET: /Direccion/Create
        public ActionResult Create()
        {

           
          // ViewBag.departamentoId = new SelectList(db.Ubigeo.OrderBy(a=> a.descripcion).Where(a=>a.codigo.Substring(2,4) == "0101"), "codigo", "descripcion") ;
           //ViewBag.provinciaId = new SelectList(db.Ubigeo.OrderBy(a => a.descripcion).Where(a => a.codigo.Substring(2, 4) == "0101"), "codigo", "descripcion");
           ViewBag.ubigeoId = new SelectList(db.Ubigeo.OrderBy(a=> a.descripcion).Where(a=>a.codigo.Substring(2,4) == "0101"), "ubigeoId", "descripcion") ;

           ViewBag.departamentoId = new SelectList(db.Ubigeo.OrderBy(a => a.descripcion).Where(a => a.codigo.Substring(2, 4) == "0000"), "codigo", "descripcion");
           ViewBag.provinciaId = new SelectList(db.Ubigeo.OrderBy(a => a.descripcion).Where(a => a.codigo.Substring(3, 1) != "0").Where(a => a.codigo.Substring(4, 2) != "00"), "codigo", "descripcion");

            return View();
        }

        // POST: /Direccion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="direccionId,descripcion,ubigeoId,usuarioCreacion,usuarioModificacion,fechaCreacion,fechaModificacion")] Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                db.Direcciones.Add(direccion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ubigeoId = new SelectList(db.Ubigeo, "ubigeoId", "descripcion", direccion.ubigeoId);
            return View(direccion);
        }

        // GET: /Direccion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Direccion direccion = db.Direcciones.Find(id);
            if (direccion == null)
            {
                return HttpNotFound();
            }

            ViewBag.departamentoId = new SelectList(db.Ubigeo.OrderBy(a => a.descripcion).Where(a => a.codigo.Substring(2, 4) == "0101"), "codigo", "descripcion", direccion.Ubigeo.codigo.Substring(0, 2) + "0101");
            ViewBag.provinciaId = new SelectList(db.Ubigeo.OrderBy(a => a.descripcion).Where(a => a.codigo.Substring(0, 2) == direccion.Ubigeo.codigo.Substring(0, 2) && a.codigo.Substring(4, 2) == "01"), "codigo", "descripcion", direccion.Ubigeo.codigo.Substring(0, 4) + "01");
            ViewBag.ubigeoId = new SelectList(db.Ubigeo.OrderBy(a => a.descripcion).Where(a => a.codigo.Substring(0, 4) == direccion.Ubigeo.codigo.Substring(0, 4)), "ubigeoId", "descripcion", direccion.ubigeoId);




            return View(direccion);
        }

        // POST: /Direccion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="direccionId,descripcion,ubigeoId")] Direccion direccion)
        {
            if (ModelState.IsValid)
            {

                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

                direccion.fechaModificacion = cstTime;
                direccion.usuarioModificacion = User.Identity.Name;

                db.Entry(direccion).State = EntityState.Modified;
                db.Entry(direccion).Property(x => x.fechaCreacion).IsModified = false;
                db.Entry(direccion).Property(x => x.usuarioCreacion).IsModified = false;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ubigeoId = new SelectList(db.Ubigeo, "ubigeoId", "codigo", direccion.ubigeoId);
            return View(direccion);
        }

        // GET: /Direccion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Direccion direccion = db.Direcciones.Find(id);
            if (direccion == null)
            {
                return HttpNotFound();
            }
            return View(direccion);
        }

        // POST: /Direccion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Direccion direccion = db.Direcciones.Find(id);
            db.Direcciones.Remove(direccion);
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
        public JsonResult GetDireccionesByCliente(int? id)
        {


            //var direcciones =  db.Direcciones.Include(p => p.ClienteDirecciones).Where(a => a.ClienteDirecciones.FirstOrDefault().clienteId == id).Select(p => new { direccionId = p.direccionId, descripcion = p.descripcion + " (" + p.ClienteDirecciones.FirstOrDefault().TipoDireccion.descripcion.Substring(0,1) + ")" }).ToList();

            var direcciones = db.ClienteDireccion.Where(a => a.clienteId == id).ToList();
            //var direcciones = db.Direcciones.Where(a => a.ClienteDirecciones.Find(id)).ToList();
                                    //select new
                                   //{
                                   //    direccionId = q.direccionId,
                                   //    descripcion = q.descripcion 
                                   //};
            var query = from q in direcciones
            select new
            {
                direccionId = q.direccionId,
                descripcion = q.Direccion.descripcion + "(" + q.TipoDireccion.descripcion.Substring(0,1) + ")"
            };



            //var direcciones = db.Direcciones.Include(p => p.ClienteDirecciones).Where(a => a.ClienteDirecciones.FirstOrDefault().clienteId == id).Select(p => new { direccionId = p.direccionId, descripcion = p.descripcion }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}
