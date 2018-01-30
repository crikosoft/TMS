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
    public class ProductoController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /Producto/
        public ActionResult Index()
        {
            var productos = db.Productos.Include(p => p.Categoria).Include(p => p.UnidadMedida);
            return View(productos.ToList());
        }

        // GET: /Producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: /Producto/Create
        public ActionResult Create()
        {
            ViewBag.categoriaId = new SelectList(db.Categorias, "categoriaId", "nombre");
            ViewBag.unidadMedidaId = new SelectList(db.UnidadMedida, "unidadMedidaId", "nombre");
            return View();
        }

        // POST: /Producto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="productoId,nombre,descripcion,unidadMedidaId,categoriaId,usuarioCreacion,usuarioModificacion,fechaCreacion,fechaModificacion")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.categoriaId = new SelectList(db.Categorias, "categoriaId", "nombre", producto.categoriaId);
            ViewBag.unidadMedidaId = new SelectList(db.UnidadMedida, "unidadMedidaId", "nombre", producto.unidadMedidaId);
            return View(producto);
        }

        // GET: /Producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoriaId = new SelectList(db.Categorias, "categoriaId", "nombre", producto.categoriaId);
            ViewBag.unidadMedidaId = new SelectList(db.UnidadMedida, "unidadMedidaId", "nombre", producto.unidadMedidaId);
            return View(producto);
        }

        // POST: /Producto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="productoId,nombre,descripcion,unidadMedidaId,categoriaId")] Producto producto)
        {
            if (ModelState.IsValid)
            {

                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

                producto.fechaModificacion = cstTime;
                producto.usuarioModificacion = User.Identity.Name;

                db.Entry(producto).State = EntityState.Modified;
                db.Entry(producto).Property(x => x.fechaCreacion).IsModified = false;
                db.Entry(producto).Property(x => x.usuarioCreacion).IsModified = false;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoriaId = new SelectList(db.Categorias, "categoriaId", "nombre", producto.categoriaId);
            ViewBag.unidadMedidaId = new SelectList(db.UnidadMedida, "unidadMedidaId", "nombre", producto.unidadMedidaId);
            return View(producto);
        }

        // GET: /Producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: /Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productos.Find(id);
            db.Productos.Remove(producto);
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
